import React, { Component } from 'react';
import PropTypes from 'prop-types';
import NumUpDownsmall from '../../StdComponents/NumUpDown_small';

class Spielabschnitt extends Component {
  constructor(props) {
    super(props);
    this.state = {
      S09: 0,
    };
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  handleClick(e) {
    if (window.location.search.includes('?')) {
      if (e.funcID === '+') {
        this.props.websocketSend({
          Domain: 'KO', Type: 'bef', Command: 'VarCount', Value1: e.variable, Value2: '+',
        });
      }

      if (e.funcID === '-') {
        this.props.websocketSend({
          Domain: 'KO', Type: 'bef', Command: 'VarCount', Value1: e.variable, Value2: '-',
        });
      }
    } else {
      if (e.funcID === '+') {
        this.setState((prevState) => ({
          [e.variable]: !prevState[e.variable] + 1,
        }));
      }

      if (e.funcID === '-' && this.state[e.variable] > 0) {
        this.setState((prevState) => ({
          [e.variable]: !prevState[e.variable] - 1,
        }));
      }
    }
  }

  reset() {
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'VarReset', Value1: 'S09',
    });

    this.setState({ S09: 0 });
  }

  render() {
    let T1 = 0;
    if (window.location.search.includes('?')) {
      T1 = this.getVarWert('S09');
    } else {
      T1 = this.state.S09;
    }

    return (
      <div className="d-flex justify-content-center">
        <NumUpDownsmall
          variable="S09"
          count={T1}
          askPlayer="Fasle"
          click={this.handleClick.bind(this)}
        />
      </div>
    );
  }
}

Spielabschnitt.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Spielabschnitt;
