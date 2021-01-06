import React, { Component } from 'react';
import PropTypes from 'prop-types';
import NumUpDown from '../../StdComponents/NumUpDown';

class Saetze extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  handleClick({ variable = '', funcID = '' } = {}) {
    if (funcID === '+') {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: 'VarCount', Value1: variable, Value2: '+',
      });
    }

    if (funcID === '-') {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: 'VarCount', Value1: variable, Value2: '-',
      });
    }
  }

  reset() {
    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'VarReset',
      Value1: 'S51',
    });

    this.props.websocketSend({
      Domain: 'KO',
      Type: 'bef',
      Command: 'VarReset',
      Value1: 'S52',
    });
  }

  render() {
    let S1 = 0;
    let S2 = 0;
    if (window.location.search.includes('?')) {
      S1 = this.getVarWert('S51');
      S2 = this.getVarWert('S52');
    } else {
      S1 = this.state.S05;
      S2 = this.state.S06;
    }

    const m1 = this.getVarWert('S01');
    const m2 = this.getVarWert('S02');

    return (
      <div>
        <div className="container p-0">

          <div className="row p-0">
            <div className="col d-flex justify-content-around p-0">

              <div className="d-inline p-0">
                <span className="d-flex justify-content-center">
                  {`${m1}:`}
                </span>
                <NumUpDown
                  variable="S51"
                  count={S1}
                  playerlist={this.props.playerlistTeam1}
                  click={this.handleClick.bind(this)}
                />
              </div>

            </div>

            <div className="col d-flex justify-content-around p-0">

              <div className="d-inline p-0">
                <span className="d-flex justify-content-center">
                  {`${m2}:`}
                </span>
                <NumUpDown
                  variable="S52"
                  count={S2}
                  playerlist={this.props.playerlistTeam2}
                  click={this.handleClick.bind(this)}
                />
              </div>

            </div>

          </div>
        </div>
      </div>
    );
  }
}

Saetze.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Saetze;
