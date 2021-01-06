import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { VscTrash } from 'react-icons/vsc';
import { RiImageAddLine } from 'react-icons/ri';
import DlgFile from '../../StdComponents/DlgFile';
import Dropdown from '../../StdComponents/Dropdown';

class PictureMgr extends Component {
  constructor() {
    super();
    this.state = {
      folder: 'Allgemein',
    };
  }

  setPropValue(property, e) {
    const { value } = e.target;
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: property,
      Value2: value,
    });
  }

  handleclick(e, f) {
    // console.log(e + "|" + f);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'delPic',
      Value1: e,
      Value2: f,
    });
  }

  handleImageFile(bild) {
    // Dateiname ankündigen
    this.props.websocketSend({
      Type: 'bef',
      Command: 'strPic',
      Value1: bild.name,
      Value2: bild.folder,
    });
    // Binärdaten übertragen
    this.props.websocketSendRaw(bild.data);
  }

  filter(e) {
    this.setState({ folder: e });
  }

  render() {
    let pl = [];
    const pl2 = [];
    if (this.props.picList.length > 0) {
      pl = this.props.picList;
      for (let i = 1; i < pl.length; i += 1) { // 0:[kein Bild] daher ab 1
        const x = pl[i].split('/');
        if (x[0] === this.state.folder) {
          pl2.push(x[1]);
        }
      }
    }
    // eslint-disable-next-line arrow-body-style
    const items = pl2.map((i) => {
      return (
        <div>
          <div className="m-1 p-1 d-flex flex-row align-items-center border border-dark">
            <img
              src={`./../../../pictures/${this.state.folder}/${i}`}
              alt=""
              style={{ maxHeight: '3rem', maxWidth: '3rem', align: 'middle' }}
            />
            <div className="p-1 flex-grow-1">{i}</div>
            <button
              type="button"
              className="btn btn-outline-danger btn-icon"
              onClick={this.handleclick.bind(this, this.state.folder, i)}
            >
              <VscTrash />
            </button>
          </div>
        </div>
      );
    });

    return (

      <div className="p-1 ml-2">
        <u>Bildverwaltung</u>
        <div className="m-2">
          {/* Bild hochladen */}
          <DlgFile
            label1="Bild wählen"
            reacticon={<RiImageAddLine />}
            class="btn btn-outline-secondary btn-icon"
            toolTip="Bilder hochladen"
            modalID="Modal-P4"
            filter="image/*"
            datei={this.handleImageFile.bind(this)}
            folderWahl="yes"
          />
        </div>

        <div className="d-flex flex-row ml-1">
          <div className="mr-1">Slideshowzeit zum Bildwechsel:</div>
          <input
            style={{ width: '5em' }}
            className="form-control"
            type="number"
            min="1"
            max="60"
            value={this.props.options.filter((x) => x.Prop === 'Slideshowzeit')[0].Value}
            onChange={this.setPropValue.bind(this, 'Slideshowzeit')}
          />
          <div className="ml-1">Sekunden</div>
        </div>

        <div className="d-flex flex-row ml-1">
          <div className="mr-1">Pfad:</div>
          <Dropdown
            values={['Allgemein', 'Mannschaften', 'Slideshow']}
            value={this.state.folder}
            wahl={this.filter.bind(this)}
          />
        </div>

        <div className="d-flex flex-row align-items-baseline">
          <div className="">
            {items}
          </div>
        </div>

      </div>
    );
  }
}

PictureMgr.propTypes = {
  picList: PropTypes.oneOfType(PropTypes.object),
  options: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
  websocketSendRaw: PropTypes.func,
};

PictureMgr.defaultProps = {
  picList: [],
  options: [],
  websocketSend: () => {},
  websocketSendRaw: () => {},
};

export default PictureMgr;
