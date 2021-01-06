import React, { Component } from 'react';
import PropTypes from 'prop-types';
import TurnierInfo from './Turnierinfo';
import Mannschaftsselector from './Mannschaftsselector';
import '../../App.css';

class Mannschaften extends Component {
  constructor(props) {
    super(props);
    this.state = {
      modus: 'Standard',
    };
  }

  onMannWahl(mNr, e) {
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'SetMannschaft', Value1: mNr, Value2: e,
    });
  }

  onManualEntry(mNr, e) {
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'SetMannschaft', Value1: mNr, Value2: '0', Value3: e.target.value,
    });
  }

  render() {
    let disable = false;
    let M1 = '';
    let M2 = '';
    let R = 0;
    let G = 0;
    let S = 0;
    let I = '';

    if (window.location.search.includes('?')) {
      if (this.props.textvariablen.length > 0) {
        M1 = this.props.textvariablen.find((x) => x.ID === 'S01').Wert;
        M2 = this.props.textvariablen.find((x) => x.ID === 'S02').Wert;
      }
    }

    if (this.props.turnierID !== 0) {
      disable = true;
      if (this.props.textvariablen.length > 0) {
        R = this.props.textvariablen.find((x) => x.ID === 'S17').Wert;
        G = this.props.textvariablen.find((x) => x.ID === 'S30').Wert;
        S = this.props.textvariablen.find((x) => x.ID === 'S29').Wert;
        I = this.props.textvariablen.find((x) => x.ID === 'S20').Wert;
      }
    }

    return (
      <div className="col p-1 m-0">

        <div className="container p-0">
          <div className="row justify-content-between">
            <div className="d-inline p-0">
              aktuelles Spiel:
            </div>
            <div className="p-0 align-content-end">
              {TurnierInfo(R, G, S)}
            </div>
          </div>
        </div>

        <div className="container p-0">
          <div className="row">
            <div className="col p-0">
              <Mannschaftsselector
                Titel="Mannschaft1"
                modus={this.state.modus}
                teamList={this.props.teamList}
                onMannWahl={this.onMannWahl.bind(this, 'A')}
                onNameTyped={this.onManualEntry.bind(this, 'A')}
                mannschaft={M1}
                disable={disable}
              />
            </div>
            <div className="col p-0">
              <Mannschaftsselector
                Titel="Mannschaft2"
                modus={this.state.modus}
                teamList={this.props.teamList}
                onMannWahl={this.onMannWahl.bind(this, 'B')}
                onNameTyped={this.onManualEntry.bind(this, 'B')}
                mannschaft={M2}
                disable={disable}
              />
            </div>
          </div>
          <div className="bg-light border border-dark rounded mt-1 pl-1 text-left">
            {I}
          </div>
        </div>

      </div>
    );
  }
}

Mannschaften.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  teamList: PropTypes.arrayOf(PropTypes.object).isRequired,
  turnierID: PropTypes.string.isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Mannschaften;
