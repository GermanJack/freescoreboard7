import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash, GoSync } from 'react-icons/go';
import { RiImageAddLine } from 'react-icons/ri';
import Table from '../StdComponents/Table';
import DlgWarning from '../StdComponents/DlgWarning';
import DlgName from '../StdComponents/DlgName';
import DlgFile from '../StdComponents/DlgFile';

class TeamList extends Component {
  constructor(props) {
    super(props);
    this.state = {
      iconSize: '1.5em',
    };
  }

  newObj(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'TeamNew', Team: e });
  }

  delObj() {
    if (this.props.team.Name === 'Heim' || this.props.team.Name === 'Gast') {
      // eslint-disable-next-line no-alert
      alert('Die Mannschaften Heim und Gast können nicht gelöscht werden.');
      return;
    }

    this.props.onDel(this.props.team.ID);
  }

  handleRefreshClick() {
    this.props.websocketSend({ Type: 'req', Command: 'Mannschaften' });
  }

  rowClick(ID) {
    this.props.objektWahl(ID);
  }

  ManNamen() {
    const ml = [];
    for (let i = 0; i < this.props.teamList.length; i += 1) {
      ml.push(this.props.teamList[i].Name);
    }
    return ml;
  }

  handleImageFile(bild) {
    // Dateiname ankündigen
    this.props.websocketSend({
      Type: 'bef',
      Command: 'strPic',
      Value1: bild.name,
    });
    // Binärdaten übertragen
    this.props.websocketSendRaw(bild.data);
  }

  render() {
    const ManNamen = this.ManNamen();
    const manName = this.props.team ? this.props.team.Name : '';
    const manID = this.props.team ? this.props.team.ID : '';

    return (
      <div className="text-white border border-fsc">
        <div className="bg-secondary pl-1 pr-1">Mannschaften</div>
        <div className="p-1">

          <div className="d-flex flex-row mb-1">

            {/* neues Objekt */}
            <DlgName
              label1="Name für neue Team:"
              values={ManNamen}
              icon={<GoPlus size={this.state.iconSize} />}
              toolTip="neue Mannschaft"
              modalID="Modal-S1"
              allowSpace
              name={this.newObj.bind(this)}
              class="btn btn-outline-secondary btn-icon btn-sm mr-1"
            />

            {/* objekt löschen */}
            <DlgWarning
              label1={`Mannschaft ${manName} unwiederruflich löschen?`}
              reacticon={<GoDash size={this.state.iconSize} />}
              toolTip="Mannschaft löschen"
              modalID="Modal-OW1"
              name={this.delObj.bind(this)}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm mr-1"
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

            {/* Mannschaften exportieren */}
            <DownloadJSon
              json={data}
              filename="Mannschaften.json"
              toolTip="csv Download"
            />

            {/* Bild hochladen */}
            <DlgFile
              label1="Bild wählen"
              reacticon={<RiImageAddLine size={this.state.iconSize} />}
              class="btn btn-outline-secondary btn-icon btn-sm mr-1"
              toolTip="Bilder hochladen"
              modalID="Modal-P4"
              filter="image/gif, image/jpeg, image/png"
              datei={this.handleImageFile.bind(this)}
              folderWahl="yes"
            />

          </div>

          <div className="d-flex flex-row">
            <Table
              daten={this.props.teamList}
              cols={[{ Column: 'Name', Label: 'Name' }]}
              chk="Selected"
              chkid={[manID]}
              radiogrp="Mannschaft"
              rowClick={this.rowClick.bind(this)}
            />
          </div>
        </div>
      </div>
    );
  }
}

TeamList.propTypes = {
  team: PropTypes.oneOfType(PropTypes.object),
  teamList: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
  websocketSendRaw: PropTypes.func,
  onDel: PropTypes.func,
  objektWahl: PropTypes.func,
};

TeamList.defaultProps = {
  team: null,
  teamList: [],
  websocketSend: () => {},
  websocketSendRaw: () => {},
  onDel: () => {},
  objektWahl: () => {},
};

export default TeamList;
