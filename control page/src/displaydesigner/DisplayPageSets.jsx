import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash, GoSync } from 'react-icons/go';
import { MdContentCopy } from 'react-icons/md';
import { BiRename } from 'react-icons/bi';
import { AiOutlineFontSize } from 'react-icons/ai';
import { RiImageAddLine } from 'react-icons/ri';
import DropdownWithID from '../StdComponents/DropdownWithID';
import DlgName from '../StdComponents/DlgName';
import DlgFile from '../StdComponents/DlgFile';
import DlgWarning from '../StdComponents/DlgWarning';

class DisplayPageSets extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleRefreshClick() {
    this.props.websocketSend({ Type: 'req', Command: 'PageSets' });
  }

  newPageSet(name) {
    // console.log(name);
    this.props.websocketSend({ Type: 'bef', Command: 'NewPageSet', Value1: name });
  }

  copyPageSet(name) {
    // console.log(name);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'CopyPageSet',
      PageSet: this.props.profil.ID,
      Value1: name,
    });
  }

  renamePageSet(name) {
    // console.log(name);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'RenamePageSet',
      PageSet: this.props.profil.ID,
      Value1: name,
    });
  }

  delPageSet() {
    if (this.props.PageSets.length === 1) {
      // eslint-disable-next-line no-alert
      alert('Das letzte PageSet kann nicht gelöscht werden!');
      return;
    }
    this.props.websocketSend({
      Type: 'bef',
      Command: 'DelPageSet',
      PageSet: this.props.profil.ID,
    });
  }

  handleImageFile(bild) {
    // pre inform File name
    this.props.websocketSend({
      Type: 'bef',
      Command: 'strPic',
      PageSet: this.props.profil.ID,
      Value1: bild.name,
    });
    // send binary datat
    this.props.websocketSendRaw(bild.data);
  }

  handleFontFile(font) {
    // pre inform File name
    this.props.websocketSend({
      Type: 'bef',
      Command: 'strFont',
      PageSet: this.props.profil.ID,
      Value1: font.name,
    });
    // send binary datat
    this.props.websocketSendRaw(font.data);
  }

  pageSetSelection(e) {
    const id = parseInt(e, 10);
    const ps = this.props.PageSets.find((x) => x.ID === id);
    this.props.PageSetSelection(ps);
  }

  render() {
    const pl = [];
    for (let i = 0; i < this.props.PageSets.length; i += 1) {
      pl.push({
        ID: this.props.PageSets[i].ID.toString(),
        Text: this.props.PageSets[i].PageSetName,
      });
    }

    let idstr = '';
    if (this.props.profil !== '') {
      idstr = this.props.profil.ID.toString();
    }
    return (
      <div className="text-white border border-dark">
        <div className="bg-secondary pl-1 pr-1">Anzeigeprofil</div>
        <div className="p-1">
          <div className="d-flex flex-row pb-1">
            <DropdownWithID
              className=""
              values={pl}
              value={idstr}
              wahl={this.pageSetSelection.bind(this)}
            />

            {/* refresh */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm"
              // data-toggle="tooltip"
              data-placement="right"
              title="refresh"
              onClick={this.handleRefreshClick.bind(this)}
            >
              <GoSync size="1.2em" />
            </button>
          </div>

          <div className="d-flex flex-row">
            {/* new Page set */}
            <DlgName
              label1="Name für neues Anzeigeprofil:"
              text=""
              values={this.props.PageSets}
              icon={<GoPlus size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              iconAltText="+"
              toolTip="neues Anzeigeprofil"
              modalID="Modal-P1"
              name={this.newPageSet.bind(this)}
            />

            {/* delete PageSet */}
            <DlgWarning
              label1={`Anzeigeprofil ${this.props.profil.PageSetName} unwiederruflich löschen?`}
              icon=""
              reacticon={<GoDash size="1.2em" />}
              iconAltText="-"
              toolTip="Anzeigeprofil löschen"
              modalID="Modal-PW1"
              name={this.delPageSet.bind(this)}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm"
            />

            {/* copy PageSet */}
            <DlgName
              label1="Name für kopiertes Anzeigeprofil:"
              text={this.props.profil.PageSetName}
              values={this.props.PageSets}
              icon={<MdContentCopy size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="Anzeigeprofil kopieren"
              modalID="Modal-P2"
              name={this.copyPageSet.bind(this)}
            />

            {/* rename PageSet */}
            <DlgName
              label1="neuer Name für Anzeigeprofil:"
              text={this.props.profil.PageSetName}
              values={this.props.PageSets}
              icon={<BiRename size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="Anzeigeprofil umbenennen"
              modalID="Modal-P3"
              name={this.renamePageSet.bind(this)}
            />

            {/* upload picture */}
            <DlgFile
              label1="Bild wählen"
              reacticon={<RiImageAddLine size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="Bilder hochladen"
              modalID="Modal-P4"
              filter="image/gif, image/jpeg, image/png"
              datei={this.handleImageFile.bind(this)}
              folderWahl="yes"
            />

            {/* upload Font */}
            <DlgFile
              label1="Schriftart wählen"
              reacticon={<AiOutlineFontSize size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="Schriftart hochladen"
              modalID="Modal-P5"
              filter=".ttf, .otf, .woff"
              datei={this.handleFontFile.bind(this)}
            />

          </div>
        </div>
      </div>
    );
  }
}

DisplayPageSets.propTypes = {
  PageSets: PropTypes.arrayOf(PropTypes.object).isRequired,
  profil: PropTypes.oneOf(PropTypes.object).isRequired,
  PageSetSelection: PropTypes.func.isRequired,
  websocketSend: PropTypes.func.isRequired,
  websocketSendRaw: PropTypes.func.isRequired,
};

export default DisplayPageSets;
