import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { FaFootballBall, FaArrowLeft, FaArrowRight } from 'react-icons/fa';
import NumUpDownsmall from '../../StdComponents/NumUpDown_small';

class AFootball extends Component {
  constructor(props) {
    super(props);
    this.state = {
      pre: 1,
    };
  }

  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '0';
  }

  reset() {
    this.props.websocketSend({
      Domain: 'KO', Type: 'bef', Command: 'ResetAFootball',
    });
  }

  clickPossession() {
    if (this.state.pre === 1) {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: 'SetPossession', Value1: '2',
      });
      this.setState({ pre: 2 });
    } else {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: 'SetPossession', Value1: '1',
      });
      this.setState({ pre: 1 });
    }
  }

  handleClick({ variable = '', funcID = '', playerID = '' } = {}) {
    if (funcID === '+') {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: 'VarCount', Value1: variable, Value2: '+', Value3: playerID,
      });
    }

    if (funcID === '-') {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: 'VarCount', Value1: variable, Value2: '-', Value3: playerID,
      });
    }
  }

  render() {
    let ball1 = <FaFootballBall size="1.5em" color="#b30000" />;
    let ball2 = <FaFootballBall size="1.5em" color="#b30000" />;
    let arrow = <FaArrowRight size="1.5em" />;
    if (this.state.pre === 1) {
      ball2 = '';
      arrow = <FaArrowRight size="1.5em" />;
    } else {
      ball1 = '';
      arrow = <FaArrowLeft size="1.5em" />;
    }

    const S39 = this.getVarWert('S39');
    const S40 = this.getVarWert('S40');
    const S42 = this.getVarWert('S42');
    const S43 = this.getVarWert('S43');
    const S41 = this.getVarWert('S41');

    const m1 = this.getVarWert('S01');
    const m2 = this.getVarWert('S02');

    return (
      <div>
        <div className="d-flex flex-row justify-content-around">
          <div className="d-inline p-0">
            <span className="d-flex justify-content-center">{m1}</span>
            <div className="d-flex justify-content-center border border-dark p-1">{ball1}</div>
          </div>

          <div className="d-inline p-0">
            <butten
              type="butten"
              className="btn btn-primary border-dark p-1 mt-4"
              onClick={this.clickPossession.bind(this)}
            >
              {arrow}
            </butten>
          </div>

          <div className="d-inline p-0">
            <span className="d-flex justify-content-center">{m2}</span>
            <div className="d-flex justify-content-center border border-dark p-1">{ball2}</div>
          </div>
        </div>

        <div className="d-flex flex-row justify-content-around">
          <div className="d-inline p-0">
            <span className="d-flex justify-content-center">Time out Left</span>
            <NumUpDownsmall
              variable="S39"
              count={S39}
              click={this.handleClick.bind(this)}
            />
          </div>
          <div className="d-inline p-0">
            <span className="d-flex justify-content-center">Time out Left</span>
            <NumUpDownsmall
              variable="S40"
              count={S40}
              click={this.handleClick.bind(this)}
            />
          </div>
        </div>

        <div className="d-flex flex-row justify-content-around">
          <div className="d-inline p-0">
            <span className="d-flex justify-content-center">Down</span>
            <NumUpDownsmall
              variable="S42"
              count={S42}
              click={this.handleClick.bind(this)}
            />
          </div>
          <div className="d-inline p-0">
            <span className="d-flex justify-content-center">Yards to go</span>
            <NumUpDownsmall
              variable="S43"
              count={S43}
              click={this.handleClick.bind(this)}
            />
          </div>
          <div className="d-inline p-0">
            <span className="d-flex justify-content-center">Ball On</span>
            <NumUpDownsmall
              variable="S41"
              count={S41}
              click={this.handleClick.bind(this)}
            />
          </div>
        </div>
      </div>
    );
  }
}

AFootball.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default AFootball;
