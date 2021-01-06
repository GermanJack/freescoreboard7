import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoPlus, GoDash, GoSync } from 'react-icons/go';
import { MdContentCopy } from 'react-icons/md';
import { BiRename } from 'react-icons/bi';
import { AiOutlinePicture } from 'react-icons/ai';
import DropdownWithID from '../StdComponents/DropdownWithID';
import DlgName from '../StdComponents/DlgName';
import DlgWarning from '../StdComponents/DlgWarning';
import DlgColorPicker from '../StdComponents/DlgColorPicker';
import DlgPicSelection from '../StdComponents/DlgPicSelection';
import DropdownBildMod from '../StdComponents/DropdownBildMod';

class DispalyPages extends Component {
  constructor(props) {
    super(props);
    this.grdRef = React.createRef();
    this.state = {
    };
  }

  handleRefreshClick() {
    this.props.websocketSend({
      Type: 'req',
      Command: 'Pages',
      PageSet: this.props.profil.ID,
    });
  }

  newPage(name) {
    // console.log(name);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'NewPage',
      PageSet: this.props.profil.ID,
      Value1: name,
    });
  }

  copyPage(name) {
    // console.log(name);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'CopyPage',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
      Value1: name,
    });
  }

  renamePage(name) {
    // console.log(name);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'RenamePage',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
      Value1: name,
    });
  }

  delPage() {
    if (this.props.pages.length === 1) {
      // eslint-disable-next-line no-alert
      alert('Die letzte Page kann nicht gelöscht werden!');
      return;
    }

    this.props.websocketSend({
      Type: 'bef',
      Command: 'DelPage',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
    });
  }

  handleColorChange(color) {
    this.props.onPageStyleChange('background-color', color);
  }

  handlePicSelection(e) {
    this.props.onPageStyleChange('background-image', `url('../../pictures/${e}')`);
  }

  handlePicModChange(e) {
    if (e === 'repeat') {
      this.props.onPageStyleChange('background-repeat', 'repeat');
      this.props.onPageStyleChange('background-position', 'initial');
      this.props.onPageStyleChange('background-size', 'auto');
    } else {
      this.props.onPageStyleChange('background-repeat', 'no-repeat');
      this.props.onPageStyleChange('background-position', 'center');
      this.props.onPageStyleChange('background-size', e);
    }
  }

  handleMColorChange(color) {
    this.props.markColor(color);
  }

  handleGridChange(e) {
    const grid = e.target.value;
    this.props.GridChange(grid);
  }

  handleDebugClick() {
    if (navigator.userAgent === 'CEF') {
      window.nativeHost.devtools();
    } else {
      // eslint-disable-next-line no-debugger
      debugger;
    }
  }

  pageSelection(e) {
    const id = parseInt(e, 10);
    const p = this.props.pages.find((x) => x.ID === id);
    this.props.pageSelection(p);
  }

  render() {
    // console.log(this.props.pages);
    let styl = '';
    if (this.props.page) {
      styl = JSON.parse(this.props.page.Style);
    }

    const pl = [];
    for (let i = 0; i < this.props.pages.length; i += 1) {
      pl.push({
        ID: this.props.pages[i].ID.toString(),
        Text: this.props.pages[i].PageName,
      });
    }

    return (
      <div className="text-white border border-dark">
        <div className="bg-secondary pl-1 pr-1">
          Anzeigeseite
        </div>
        <div className="p-1">

          {/* row 1 */}
          <div className="d-flex flex-row pb-1">
            <DropdownWithID
              className=""
              values={pl}
              value={this.props.page ? this.props.page.ID.toString() : ''}
              wahl={this.pageSelection.bind(this)}
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

          {/* row 2 */}
          <div className="d-flex flex-row mb-1">
            {/* new Page */}
            <DlgName
              label1="Name für neue Anzeigeseite:"
              text=""
              values={this.props.pages}
              icon={<GoPlus size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="neue Seite"
              modalID="Modal-S1"
              name={this.newPage.bind(this)}
            />

            {/* delete Page */}
            <DlgWarning
              label1={`Anzeigeseite ${this.props.page ? this.props.page.PageName : ''} unwiederruflich löschen?`}
              icon=""
              reacticon={<GoDash size="1.2em" />}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm"
              iconAltText="-"
              toolTip="Seite löschen"
              modalID="Modal-SW1"
              name={this.delPage.bind(this)}
            />

            {/* copy Page */}
            <DlgName
              label1="Name für kopierte Anzeigeseite:"
              text={this.props.page ? this.props.page.PageName : ''}
              values={this.props.pages}
              icon={<MdContentCopy size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="Seite kopieren"
              modalID="Modal-S2"
              name={this.copyPage.bind(this)}
            />

            {/* rename Page */}
            <DlgName
              label1="neuer Name für Anzeigeseite:"
              text={this.props.page ? this.props.page.PageName : ''}
              values={this.props.pages}
              icon={<BiRename size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="Seite umbenennen"
              modalID="Modal-S3"
              name={this.renamePage.bind(this)}
            />

            {/* Mark color */}
            <DlgColorPicker
              toolTip="Markierungsfarbe"
              color={this.props.page ? this.props.page.MarkColor : 'rgba(31, 223, 14, 1)'}
              onChange={this.handleMColorChange.bind(this)}
            />

            {/* Raster */}
            <input
              title="Raster"
              type="number"
              className=""
              style={{ width: '50px' }}
              min="1"
              max="40"
              value={this.props.page ? this.props.page.Grid : 20}
              ref={this.grdRef}
              onChange={this.handleGridChange.bind(this)}
            />

          </div>

          {/* row 3 */}
          <div className="d-flex flex-row">

            {/* BackGroundColor */}
            <DlgColorPicker
              toolTip="Hintergrundfarbe"
              color={styl ? styl['background-color'] : ''}
              onChange={this.handleColorChange.bind(this)}
            />

            {/* BackGroundPicture */}
            <DlgPicSelection
              label1="Bildwahl:"
              reacticon={<AiOutlinePicture size="1.2em" />}
              class="btn btn-outline-secondary btn-icon btn-sm"
              data-toggle="tooltip"
              data-placement="right"
              toolTip="Hintergrundbild"
              modalID="Modal-S4"
              values={this.props.picList}
              selection={this.handlePicSelection.bind(this)}
            />

            {/* Background Picture Mode */}
            <DropdownBildMod
              value={styl['background-size'] ? styl['background-size'] : 'contain'}
              onChange={this.handlePicModChange.bind(this)}
            />

          </div>
        </div>
      </div>
    );
  }
}

DispalyPages.propTypes = {
  pages: PropTypes.arrayOf(PropTypes.string).isRequired,
  profil: PropTypes.oneOf(PropTypes.object).isRequired,
  picList: PropTypes.arrayOf(PropTypes.object).isRequired,
  markColor: PropTypes.func.isRequired,
  page: PropTypes.string.isRequired,
  pageSelection: PropTypes.func.isRequired,
  websocketSend: PropTypes.func.isRequired,
  onPageStyleChange: PropTypes.func.isRequired,
  GridChange: PropTypes.func.isRequired,
};

export default DispalyPages;
