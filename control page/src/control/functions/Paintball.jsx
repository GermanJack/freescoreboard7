import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Pic5Counter from './Pic5Counter';

class Paintball extends Component {
  constructor(props) {
    super(props);
    this.state = {
      S46: 5,
      S47: 5,
    };
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  handleClick(variable, funcID) {
    if (window.location.search.includes('?')) {
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
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'VarReset', Value1: 'S46',
    });

    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'VarReset', Value1: 'S47',
    });

    this.setState({ S46: 5 });
    this.setState({ S47: 5 });
  }

  render() {
    let T1 = 0;
    let T2 = 0;
    if (window.location.search.includes('?')) {
      T1 = this.getVarWert('S46');
      T2 = this.getVarWert('S47');
    } else {
      T1 = this.state.S46;
      T2 = this.state.S47;
    }

    return (
      <div>
        <div className="container p-0">

          <div className="row p-0">
            <div className="col d-flex justify-content-around p-0">

              <div className="d-inline p-0">
                <span className="d-flex justify-content-center">Mannschaft1:</span>
                <Pic5Counter
                  picVariables={this.props.picVariables}
                  variable="S46"
                  count={T1}
                  click={this.handleClick.bind(this)}
                />
              </div>

            </div>

            <div className="col d-flex justify-content-around p-0">

              <div className="d-inline p-0">
                <span className="d-flex justify-content-center">Mannschaft2:</span>
                <Pic5Counter
                  picVariables={this.props.picVariables}
                  variable="S47"
                  count={T2}
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

Paintball.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  picVariables: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Paintball;
