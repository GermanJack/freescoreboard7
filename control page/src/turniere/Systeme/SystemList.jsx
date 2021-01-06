import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash, GoSync } from 'react-icons/go';
import Table from '../../StdComponents/Table';
import DlgWarning from '../../StdComponents/DlgWarning';
import DlgSystemNeu from './DlgSystemNeu';
import SystemDetails from './SystemDetails';

class SystemList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      TurnierID: 0,
      iconSize: '1.5em',
    };
  }

  rowClick(ID) {
    this.setState({ TurnierID: ID });
  }

  clickDel() {
    this.setState({ TurnierID: 0 });
    this.props.websocketSend({ Type: 'bef', Command: 'DelSystem', Value1: this.state.TurnierID });
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

  render() {
    const Systeme = this.props.turniere ? this.props.turniere.filter((x) => x.Kopf.Turniertyp === 'System').map((y) => y.Kopf) : null;
    const SystemNamen = this.namensliste('System');
    let System = null;
    if (this.state.TurnierID !== 0) {
      System = this.props.turniere.filter((x) => x.Kopf.ID === this.state.TurnierID);
    }

    return (
      <div className="border border-fsc">
        <div className="text-white bg-secondary pl-1 pr-1">Turniersysteme</div>
        <div className="p-1">

          <div className="d-flex flex-row mb-1">

            {/* neues Objekt */}
            <DlgSystemNeu
              label1="neues Turniersystem"
              toolTip="neues Turniersystem"
              modalID="Modal-Sys1"
              namensliste={SystemNamen}
              websocketSend={this.props.websocketSend.bind(this)}
              reacticon={<GoPlus size={this.state.iconSize} />}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm mr-1"
            />

            {/* objekt löschen */}
            <DlgWarning
              modalID="delsys1"
              label1="Turniersystem unwiederruflich löschen?"
              toolTip="Turniersystem löschen"
              name={this.clickDel.bind(this)}
              reacticon={<GoDash size={this.state.iconSize} />}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm mr-1"
            />

            {/* refresh */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm mr-1"
              // data-toggle="tooltip"
              data-placement="right"
              title="refresh"
              // onClick={this.handleRefreshClick.bind(this)}
            >
              <GoSync size={this.state.iconSize} />
            </button>

          </div>

          <div className="d-flex flex-row">
            <div>
              <Table
                daten={Systeme}
                cols={[{ Column: 'Beschreibung', Label: 'Name' }]}
                chk="Selected"
                chkid={[this.state.TurnierID]}
                radiogrp="Syteme"
                rowClick={this.rowClick.bind(this)}
              />
            </div>
            <div className="ml-1">
              <SystemDetails
                system={System}
              />
            </div>
          </div>
        </div>
      </div>
    );
  }
}

SystemList.propTypes = {
  turniere: PropTypes.arrayOf(PropTypes.arrayOf.objects),
  websocketSend: PropTypes.func.isRequired,
};

SystemList.defaultProps = {
  turniere: [],
};

export default SystemList;
