import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { TiArrowUpOutline, TiArrowDownOutline } from 'react-icons/ti';
import { GoPlus, GoDash, GoSync } from 'react-icons/go';
import { MdContentCopy } from 'react-icons/md';
import Table from '../StdComponents/Table';
import DlgWarning from '../StdComponents/DlgWarning';
// import DlgColorPicker from './Modals/DlgColorPicker';

class DisplayObjects extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  objUp() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'UpDivs',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
      Divs: this.props.selectedObjectIDs,
    });
    this.props.iFrameRefresh();
  }

  objDown() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'DownDivs',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
      Divs: this.props.selectedObjectIDs,
    });
    this.props.iFrameRefresh();
  }

  newObj() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'NewDiv',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
    });
    this.props.iFrameRefresh();
  }

  delObj() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'DelDivs',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
      Divs: this.props.selectedObjectIDs,
    });
    this.props.refreshDivs();
  }

  divCopy() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'CopyDivs',
      PageSet: this.props.profil.ID,
      Page: this.props.page.ID,
      Divs: this.props.selectedObjectIDs,
    });
  }

  handleRefreshClick() {
    this.props.refreshDivs();
  }

  rowClick(ID, checked) {
    this.props.objectSelection(ID, checked.target.checked);
  }

  render() {
    return (
      <div className="text-white border border-dark">
        <div className="bg-secondary pl-1 pr-1">Anzeigeobjekte</div>
        <div className="p-1">

          <div className="d-flex flex-row pb-1">

            {/* divs up */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm"
              data-toggle="tooltip"
              data-placement="right"
              title="Objekt hoch"
              onClick={this.objUp.bind(this)}
            >
              <TiArrowUpOutline size="1.2em" />
            </button>

            {/* divs down */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm"
              data-toggle="tooltip"
              data-placement="right"
              title="Objekt runter"
              onClick={this.objDown.bind(this)}
            >
              <TiArrowDownOutline size="1.2em" />
            </button>

            {/* new div */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm"
              data-toggle="tooltip"
              data-placement="right"
              title="neues Objekt"
              onClick={this.newObj.bind(this)}
            >
              <GoPlus size="1.2em" />
            </button>

            {/* delete div */}
            <DlgWarning
              label1="Alle markierten Divs unwiederruflich löschen?"
              reacticon={<GoDash size="1.2em" />}
              btnclassname="btn btn-outline-secondary btn-icon btn-sm"
              toolTip="Divs löschen"
              modalID="Modal-OW1"
              name={this.delObj.bind(this)}
            />

            {/* copy div */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm"
              data-toggle="tooltip"
              data-placement="right"
              title="Objekt kopieren"
              onClick={this.divCopy.bind(this)}
            >
              <MdContentCopy size="1.2em" />
            </button>

            {/* refresh */}
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon btn-sm"
              // data-toggle="tooltip"
              data-placement="right"
              title="refresh / Markierung aufheben"
              onClick={this.handleRefreshClick.bind(this)}
            >
              <GoSync size="1.2em" />
            </button>

            {/* paste style
              <button type="button"
                className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1"
                data-toggle="tooltip"
                data-placement="right"
                title="Style einfügen"
                onClick={this.stylePaste.bind(this)}>
                <img src={require('./PasteCSS.png')} alt="p" />
              </button> */}

            {/* align divs on top */}
            {/* <button type="button"
                className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 mb-1"
                data-toggle="tooltip"
                data-placement="right"
                title="oben Ausrichten"
                onClick={this.align.bind(this)}>
                <img src={require('../Icons/icons8-align-top-16.png')} alt="p" />
              </button> */}

          </div>
          <div className="d-flex flex-row">
            <Table
              daten={this.props.allDivs}
              cols={[{ Column: 'ID', Label: 'ID' }, { Column: 'Zindex', Label: 'Z' }, { Column: 'Info', Label: 'Info' }]}
              chk="Selected"
              chkid={this.props.selectedObjectIDs}
              rowClick={this.rowClick.bind(this)}
              height="20rem"
              chktype="checkbox"
            />
          </div>
        </div>
      </div>
    );
  }
}

DisplayObjects.propTypes = {
  profil: PropTypes.oneOf(PropTypes.object).isRequired,
  allDivs: PropTypes.arrayOf(PropTypes.object).isRequired,
  page: PropTypes.string.isRequired,
  selectedObjectIDs: PropTypes.arrayOf(PropTypes.number),
  objectSelection: PropTypes.func.isRequired,
  refreshDivs: PropTypes.func.isRequired,
  websocketSend: PropTypes.func.isRequired,
  iFrameRefresh: PropTypes.func.isRequired,
};

DisplayObjects.defaultProps = {
  selectedObjectIDs: [],
};

export default DisplayObjects;
