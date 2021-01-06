import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Zeit from '../../StdComponents/Zeit';

class Zeiten extends Component {
  getVarWert(v) {
    const i = this.props.textvariablen.map((t) => t.ID).indexOf(v);
    return i !== -1 ? this.props.textvariablen[i].Wert : '00:00';
  }

  handleClick(bef, nr) {
    if (window.location.search.includes('?')) {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: bef, Value1: nr,
      });
    }
  }

  render() {
    const item = this.props.timer ? this.props.timer
      .sort((a, b) => a.Nr - b.Nr)
      .filter((y) => y.Kontrolanzeige === true)
      // eslint-disable-next-line arrow-body-style
      .map((x) => {
        return (
          <Zeit
            timer={x}
            active={
              this.props.timerObjects.length > 0
                ? this.props.timerObjects.find((z) => z.ID === x.Nr).Status
                : null
            }
            variable={x.Variable}
            wert={this.getVarWert(x.Variable)}
            click={this.handleClick.bind(this)}
            Name={x.Name}
            TimerNr={x.Nr}
            key={x.Nr}
            websocketSend={this.props.websocketSend.bind(this)}
          />
        );
      }) : null;

    return (
      <div>
        <div className="col p-1 m-0">
          {item}
        </div>
      </div>
    );
  }
}

Zeiten.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  timer: PropTypes.arrayOf(PropTypes.object).isRequired,
  timerObjects: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default Zeiten;
