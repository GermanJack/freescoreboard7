import React, { Component } from 'react';
import PropTypes from 'prop-types';
import NumUpDownsmall from '../../StdComponents/NumUpDown_small';

class Foul extends Component {
  constructor(props) {
    super(props);
    this.state = {
      S18: 0,
      S19: 0,
    };
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  reset() {
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'Foul_Reset', Value1: 'S18', Value3: '',
    });

    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'Foul_Reset', Value1: 'S19', Value3: '',
    });

    this.setState({ S18: 0 });
    this.setState({ S19: 0 });
  }

  handleClick({ variable = '', funcID = '', playerID = '' } = {}) {
    if (window.location.search.includes('?')) {
      if (funcID === '+') {
        this.props.websocketSend({
          Domain: 'KO', Type: 'bef', Command: 'Foul', Value1: variable, Value2: '+', Value3: playerID,
        });
      }

      if (funcID === '-') {
        this.props.websocketSend({
          Domain: 'KO', Type: 'bef', Command: 'Foul', Value1: variable, Value2: '-', Value3: playerID,
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

  playerRequired() {
    let y = 'False';
    if (this.props.options.length > 0) {
      y = (this.props.options.find((x) => x.Prop === 'Foulspieler')).Value;
    }
    return y;
  }

  render() {
    let T1 = 0;
    let T2 = 0;
    if (window.location.search.includes('?')) {
      T1 = this.getVarWert('S18');
      T2 = this.getVarWert('S19');
    } else {
      T1 = this.state.S18;
      T2 = this.state.S19;
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
                <NumUpDownsmall
                  variable="S18"
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
                <NumUpDownsmall
                  variable="S19"
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

Foul.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  options: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam1: PropTypes.arrayOf(PropTypes.object).isRequired,
  playerlistTeam2: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Foul;
