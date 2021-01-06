import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { IoMdArrowDropdown } from 'react-icons/io';
import TurnierInfo from './Turnierinfo';
import DlgTable from '../../StdComponents/DlgTable';
import DlgMatchChange from './DlgMatchChange';

class Turniersteuereung extends Component {
  constructor(props) {
    super(props);
    this.Name = {
      value: null,
    };

    // This binding is necessary to make `this` work in the callback
    this.handleClick = this.handleClick.bind(this);
  }

  handleClick(funcID) {
    if (navigator.userAgent === 'CEF') {
      window.nativeHost.click(funcID);
      const log = document.getElementById('divlog');
      log.innerText += `\r\n${funcID}`;
    }
  }

  handleMatchWahl(e) {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'SetNextMatch',
      Value1: e.wahl,
    });
  }

  spielUebernehmen() {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'NextToActual',
      Value1: '1',
    });
  }

  render() {
    let R = 0;
    let G = 0;
    let S = 0;
    let T1 = '';
    let T2 = '';
    let as = '0';
    let disable = true;
    const matchList = [];

    if (this.props.turnierID !== 0) {
      if (this.props.textvariablen.length > 0) {
        R = this.props.textvariablen.find((x) => x.ID === 'S48').Wert;
        G = this.props.textvariablen.find((x) => x.ID === 'S49').Wert;
        S = this.props.textvariablen.find((x) => x.ID === 'S50').Wert;
        T1 = this.props.textvariablen.find((x) => x.ID === 'S32').Wert;
        T2 = this.props.textvariablen.find((x) => x.ID === 'S33').Wert;
        as = this.props.textvariablen.find((x) => x.ID === 'S29').Wert;
      }

      disable = false;

      // liste der spiele der aktiven Runde generieren
      if (this.props.turnier.length > 0) {
        const r = this.props.turnier[0].Runden;
        const r1 = r.filter((x) => x.status === 1);
        let ru = 0;
        if (r1 !== undefined) {
          if (r1.length > 0) {
            ru = r1[0].Runde;
          }
        }

        const spiele = this.props.turnier[0].Spiele;
        const sl = spiele.filter((x) => x.Runde === ru);
        for (let i = 0; i < sl.length; i += 1) {
          if (sl[i].Status === 2) {
            matchList.push({ ID: sl[i].Spiel, Name: `Gruppe ${sl[i].Gruppe}: ${sl[i].IstMannA} - ${sl[i].IstMannB}` });
          }
        }
      }
    }

    let spielbeenden = null;
    if (as !== '0') {
      spielbeenden = (
        <DlgMatchChange
          turnierID={this.props.turnierID}
          websocketSend={this.props.websocketSend}
        />
      );
    } else {
      spielbeenden = (
        <div className="d-flex flex-column">
          <button
            type="button"
            className="btn btn-warning btn-sm border border-dark mt-1"
            data-placement="right"
            title="Spielendebehandlung"
            disabled={disable}
            onClick={this.spielUebernehmen.bind(this)}
          >
            aktives Spiel beenden / nächstes Spiel übernehmen
          </button>
        </div>
      );
    }

    return (
      <div className="container p-0">
        <div className="row justify-content-between">
          <div className="row p-0">
            <div className="pr-1">
              nächstes Spiel:
            </div>
            <DlgTable
              label1="nächstes Spiel wählen:"
              data-toggle="tooltip"
              data-placement="right"
              toolTip="nächstes Spiel wählen"
              modalID="nextMatchSel"
              data={matchList}
              wahl={this.handleMatchWahl.bind(this)}
              disabled={disable}
              reacticon={<IoMdArrowDropdown size="1.5em" />}
            />
          </div>

          <div className="p-0 justify-content-end">
            {TurnierInfo(R, G, S)}
          </div>
        </div>

        <div className="container p-0">
          <div className="row">
            <div className="col p-0">
              <div>
                Mannschaft1:
              </div>
              <div className="form-control form-control-sm">
                {T1}
              </div>
            </div>
            <div className="col p-0">
              <div>
                Mannschaft1:
              </div>
              <div className="form-control form-control-sm">
                {T2}
              </div>
            </div>
          </div>
          {spielbeenden}
        </div>
      </div>
    );
  }
}

Turniersteuereung.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object),
  turnierID: PropTypes.string,
  turnier: PropTypes.string.isRequired,
  websocketSend: PropTypes.func.isRequired,
};

Turniersteuereung.defaultProps = {
  textvariablen: [],
  turnierID: 0,
};

export default Turniersteuereung;
