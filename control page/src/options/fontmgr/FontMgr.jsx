import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { VscTrash } from 'react-icons/vsc';
import { AiOutlineFontSize } from 'react-icons/ai';
import DlgFile from '../../StdComponents/DlgFile';

class FontMgr extends Component {
  handleclick(e) {
    // console.log(e + "|" + f);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'delFont',
      Value1: e,
    });
  }

  handleFontFile(FontFile) {
    // Dateiname ankündigen
    this.props.websocketSend({ Type: 'bef', Command: 'strFont', Value1: FontFile.name });
    // Binärdaten übertragen
    this.props.websocketSendRaw(FontFile.data);
  }

  render() {
    // eslint-disable-next-line arrow-body-style
    const items = this.props.fontList.map((i) => {
      return (
        <div>
          <div className="m-1 p-1 d-flex flex-column align-items-center border border-dark">

            <div className="p-1 d-flex justify-content-start">
              <u>{i.fontfile}</u>
            </div>

            <div className="d-flex flex-column" style={{ fontFamily: i.fontname, fontSize: '200%' }}>
              <div>ABCDEFGHIJKLMNOPQRSTUVWXYZ</div>
              <div>abcdefghijklmnopqrstuvwxyz</div>
              <div>1234567890 üÜ äÄ öÖ ß & ?</div>
            </div>

            <div className="d-flex justify-content-end">
              <button type="button" className="btn btn-outline-danger btn-icon" onClick={this.handleclick.bind(this, i.fontfile)}>
                <VscTrash />
              </button>
            </div>

          </div>
        </div>
      );
    });

    return (

      <div className="p-1 ml-2">
        <u>Schriftartenverwaltung</u>
        <div className="m-2">
          {/* Audiodatei hochladen */}
          <DlgFile
            label1="Schriftdatei wählen"
            reacticon={<AiOutlineFontSize />}
            class="btn btn-outline-secondary btn-icon"
            toolTip="Schriftart hochladen"
            modalID="Modal-P4"
            filter=".ttf, .otf, .woff"
            datei={this.handleFontFile.bind(this)}
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

FontMgr.propTypes = {
  fontList: PropTypes.OfType(PropTypes.object),
  websocketSend: PropTypes.func,
  websocketSendRaw: PropTypes.func,
};

FontMgr.defaultProps = {
  fontList: [],
  websocketSend: () => {},
  websocketSendRaw: () => {},
};

export default FontMgr;
