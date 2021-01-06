import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash } from 'react-icons/go';
import { FaRegPlayCircle, FaRegStopCircle } from 'react-icons/fa';
import Table from '../../StdComponents/Table';
import DlgWarning from '../../StdComponents/DlgWarning';
import DlgTurnierNeu from './DlgTurnierNeu';
import TurnierDetailsPrint from './TurnierDetailsPrint';

class TurnierList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      TurnierID: 0,
      iconSize: '1.5em',
    };
  }

  componentDidMount() {
    if (this.props.activeTurnierID !== 0) {
      this.rowClick(this.props.activeTurnierID);
    }
  }

  rowClick(ID) {
    this.setState({ TurnierID: ID });
  }

  turnierDelete() {
    this.setState({ TurnierID: 0 });
    this.props.websocketSend({ Type: 'bef', Command: 'DelTurnier', Value1: this.state.TurnierID });
  }

  handleRefreshClick() {
    this.props.websocketSend({ Type: 'req', Command: 'TurniereKomplett' });
  }

  namensliste(e) {
    const data = [];
    const filter = e;
    for (let i = 0; i < this.props.turniere.length; i += 1) {
      if (this.props.turniere[i].Kopf.Turniertyp === filter) {
        data.push(this.props.turniere[i].Kopf.Beschreibung);
      }
    }
    return data;
  }

  changeActiveTurnier(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'SetTurnierID', Value1: e });
  }

  render() {
    const Turniere = this.props.turniere ? this.props.turniere.filter((x) => x.Kopf.Turniertyp === 'Turnier').map((y) => y.Kopf) : [];
    const Systeme = this.props.turniere ? this.props.turniere.filter((x) => x.Kopf.Turniertyp === 'System') : [];
    const TurnierNamen = this.namensliste('Turnier');

    let Turnier = null;
    if (this.state.TurnierID !== 0) {
      Turnier = this.props.turniere.filter((x) => x.Kopf.ID === this.state.TurnierID);
    }

    let activeBtn = '';
    if (this.state.TurnierID !== 0) {
      activeBtn = (
        <button
          className="btn btn-outline-success btn-sm btn-icon"
          type="button"
          title="Turnier starten"
          onClick={this.changeActiveTurnier.bind(this, this.state.TurnierID)}
        >
          <FaRegPlayCircle size="1.2em" />
        </button>
      );
    }

    if (this.state.TurnierID === this.props.activeTurnierID && this.state.TurnierID !== 0) {
      activeBtn = (
        <button
          className="btn btn-outline-warning btn-sm btn-icon"
          type="button"
          title="Turnier stoppen"
          onClick={this.changeActiveTurnier.bind(this, 0)}
        >
          <FaRegStopCircle size="1.2em" />
        </button>
      );
    }

    return (
      <div className="border border-fsc">
        <div className="text-white bg-secondary pl-1 pr-1">Turniere</div>
        <div className="p-1">

          <div className="d-flex flex-row mb-1">

            {/* neues Objekt */}
            <DlgTurnierNeu
              label1="neues Turnier"
              toolTip="neues Turnier"
              modalID="Modal-Turn21"
              namensliste={TurnierNamen}
              websocketSend={this.props.websocketSend.bind(this)}
              systeme={Systeme}
              teamList={this.props.teamList}
              reacticon={<GoPlus size={this.state.iconSize} />}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm mr-1"
            />

            {/* objekt löschen */}
            <DlgWarning
              label1="Turnier unwiederruflich löschen?"
              toolTip="Turnier löschen"
              modalID="Modal-Turn22"
              name={this.turnierDelete.bind(this)}
              reacticon={<GoDash size={this.state.iconSize} />}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm mr-1"
            />

            {/* starten/beenden */}
            {activeBtn}
          </div>

          <div className="d-flex flex-row">
            <div>
              <Table
                daten={Turniere}
                cols={[{ Column: 'Beschreibung', Label: 'Name' }]}
                chk="Selected"
                chkid={[this.state.TurnierID]}
                radiogrp="Turniere"
                rowClick={this.rowClick.bind(this)}
              />
            </div>
            <div className="ml-1">
              <TurnierDetailsPrint
                websocketSend={this.props.websocketSend.bind(this)}
                turnier={Turnier}
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

TurnierList.propTypes = {
  turniere: PropTypes.arrayOf(PropTypes.arrayOf.objects),
  teamList: PropTypes.arrayOf(PropTypes.object),
  activeTurnierID: PropTypes.number,
  websocketSend: PropTypes.func.isRequired,
};

TurnierList.defaultProps = {
  turniere: [],
  activeTurnierID: 0,
  teamList: [],
};

export default TurnierList;
