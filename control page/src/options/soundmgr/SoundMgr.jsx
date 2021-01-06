import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { VscTrash } from 'react-icons/vsc';
import { ImFileMusic } from 'react-icons/im';
import DlgFile from '../../StdComponents/DlgFile';

class SoundMgr extends Component {
  handleclick(e) {
    // console.log(e + "|" + f);
    this.props.websocketSend({
      Type: 'bef',
      Command: 'delSnd',
      Value1: e,
    });
  }

  handleSoundFile(Audiodatei) {
    // Dateiname ankündigen
    this.props.websocketSend({ Type: 'bef', Command: 'strSnd', Value1: Audiodatei.name });
    // Binärdaten übertragen
    this.props.websocketSendRaw(Audiodatei.data);
  }

  render() {
    if (this.props.soundList[0] === '[kein Ton]') {
      this.props.soundList.shift();
    }
    const items = this.props.soundList.map((i) => {
      let ex = i.split('.')[1].toLowerCase();
      if (ex === 'mp3') {
        ex = 'mpeg';
      }

      return (
        <div>
          <div className="m-1 p-1 d-flex flex-row align-items-center border border-dark">

            <div className="p-1 flex-grow-1">{i}</div>
            <audio key={i} controls>
              <track kind="captions" />
              <source src={`./../../../sounds/${i}`} type={`audio/${ex}`} />
              Your browser does not support the audio element.
            </audio>
            <button
              type="button"
              className="btn btn-outline-danger btn-icon"
              onClick={this.handleclick.bind(this, i)}
            >
              <VscTrash />
            </button>
          </div>
        </div>
      );
    });

    return (

      <div className="p-1 ml-2">
        <u>Töneverwaltung</u>
        <br />
        <div className="m-2">
          {/* Audiodatei hochladen */}
          <DlgFile
            label1="Audiodatei wählen"
            reacticon={<ImFileMusic />}
            class="btn btn-outline-secondary btn-icon"
            toolTip="Audiodatei hochladen"
            modalID="Modal-P4"
            filter="audio/*"
            datei={this.handleSoundFile.bind(this)}
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

SoundMgr.propTypes = {
  soundList: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
  websocketSendRaw: PropTypes.func,
};

SoundMgr.defaultProps = {
  soundList: [],
  websocketSend: () => {},
  websocketSendRaw: () => {},
};

export default SoundMgr;
