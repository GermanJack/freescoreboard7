import React, { Component } from 'react';
import PropTypes from 'prop-types';
import NumUpDown from '../../StdComponents/NumUpDown';

class Tore extends Component {
  constructor(props) {
    super(props);
    this.state = {
      S05: 0,
      S06: 0,
    };
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  playerRequired() {
    let y = 'False';
    if (this.props.options.length > 0) {
      y = (this.props.options.find((x) => x.Prop === 'Torschütze')).Value;
    }
    return y;
  }

  handleClick({ variable = '', funcID = '', playerID = '' } = {}) {
    if (window.location.search.includes('?')) {
      if (funcID === '+') {
        this.props.websocketSend({
          Domain: 'KO', Type: 'bef', Command: 'Tor', Value1: variable, Value2: '+', Value3: playerID,
        });
      }

      if (funcID === '-') {
        this.props.websocketSend({
          Domain: 'KO', Type: 'bef', Command: 'Tor', Value1: variable, Value2: '-', Value3: playerID,
        });
      }
    } else {
      if (funcID === '+') {
        this.setState((prevState) => ({
          [variable]: prevState[variable] + 1,
        }));
      }

      if (funcID === '-' && this.state[variable] > 0) {
        this.setState((prevState) => ({
          [variable]: prevState[variable] - 1,
        }));
      }
    }
  }

  reset() {
    // console.log('Torereset');
    this.props.websocketSend({ Domain: 'KO', Type: 'bef', Command: 'Tore_Reset' });

    this.setState({ S05: 0 });
    this.setState({ S06: 0 });
  }

  render() {
    let T1 = 0;
    let T2 = 0;
    if (window.location.search.includes('?')) {
      T1 = this.getVarWert('S05');
      T2 = this.getVarWert('S06');
    } else {
      T1 = this.state.S05;
      T2 = this.state.S06;
    }

    const askPlayer = this.playerRequired();

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
                  variable="S05"
                  count={T1}
                  askPlayer={askPlayer}
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
                  variable="S06"
                  count={T2}
                  askPlayer={askPlayer}
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

Tore.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Tore;
