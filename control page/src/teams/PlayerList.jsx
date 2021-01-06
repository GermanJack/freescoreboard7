import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash, GoSync } from 'react-icons/go';
import { FaExchangeAlt } from 'react-icons/fa';
import Table from '../StdComponents/Table';
import DlgWarning from '../StdComponents/DlgWarning';
import DlgName from '../StdComponents/DlgName';
import DlgTable from '../StdComponents/DlgTable';

class PlayerList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      iconSize: '1.2em',
    };
  }

  newObj(e) {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'PlayerNew',
      Team: this.props.team.ID,
      Player: e,
    });
  }

  delObj() {
    this.props.onDel(this.props.player.ID);
  }

  handleRefreshClick() {
    this.props.websocketSend({
      Type: 'req',
      Command: 'PlayerList',
      Team: this.props.team.ID,
    });
  }

  handlePlayerTransfer(e) {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'PlayerChange',
      Team: this.props.team.ID,
      Player: this.props.player.ID,
      Property: 'MannschaftsID',
      Value1: e,
    });
  }

  rowClick(ID) {
    this.props.objektWahl(ID);
  }

  ManNamen() {
    const ml = [];
    for (let i = 0; i < this.props.player.length; i += 1) {
      ml.push(this.props.player[i].Name);
    }
    return ml;
  }

  render() {
    const sName = this.props.player && this.props.player.Nachname ? this.props.player.Nachname : '';
    const sID = this.props.player ? this.props.player.ID : '';

    return (
      <div className="text-white border border-fsc">
        <div className="bg-secondary pl-1 pr-1">Spieler</div>
        <div className="p-1">

          <div className="d-flex flex-row mb-1">

            {/* neues Objekt */}
            <DlgName
              label1="Nachname für neuen Player:"
              values={[]}
              allowSpace
              icon={<GoPlus size={this.state.iconSize} />}
              class="btn btn-outline-secondary btn-icon btn-sm mr-1"
              toolTip="neuer Spieler"
              modalID="Modal-Sp1"
              name={this.newObj.bind(this)}
            />

            {/* objekt löschen */}
            <DlgWarning
              label1={`Spieler ${sName} unwiederruflich löschen?`}
              reacticon={<GoDash size={this.state.iconSize} />}
              toolTip="Spieler löschen"
              modalID="Modal-Sp2"
              name={this.delObj.bind(this)}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm mr-1"
            />

            {/* objekt transfer */}
            <DlgTable
              label1="neue Mannschaft:"
              className="btn btn-outline-secondary btn-icon btn-sm mr-1"
              data-toggle="tooltip"
              data-placement="right"
              toolTip="Transfer"
              reacticon={<FaExchangeAlt size={this.state.iconSize} />}
              iconAltText="<->"
              modalID="Modal-M4"
              data={this.props.teamList}
              wahl={this.handlePlayerTransfer.bind(this)}
            />

            {/* refresh */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm mr-1"
              // data-toggle="tooltip"
              data-placement="right"
              title="refresh"
              onClick={this.handleRefreshClick.bind(this)}
            >
              <GoSync size={this.state.iconSize} />
            </button>

          </div>

          <div className="d-flex flex-row">
            <Table
              daten={this.props.playerList}
              cols={[{ Column: 'Nachname', Label: 'Nachname' }, { Column: 'Vorname', Label: 'Vorname' }]}
              chk="Selected"
              chkid={[sID]}
              radiogrp="player"
              rowClick={this.rowClick.bind(this)}
            />
          </div>
        </div>
      </div>
    );
  }
}

PlayerList.propTypes = {
  team: PropTypes.oneOfType(PropTypes.object),
  player: PropTypes.oneOfType(PropTypes.object),
  teamList: PropTypes.oneOfType(PropTypes.object),
  playerList: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
  onDel: PropTypes.func,
  objektWahl: PropTypes.func,
};

PlayerList.defaultProps = {
  team: null,
  player: null,
  teamList: null,
  playerList: null,
  websocketSend: () => {},
  onDel: () => {},
  objektWahl: () => {},
};

export default PlayerList;
