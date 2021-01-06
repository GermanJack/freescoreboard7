import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Zeit2 from '../../StdComponents/Zeit2';

class ZeitenNachspielzeit extends Component {
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

  reset() {
    if (window.location.search.includes('?')) {
      this.props.websocketSend({
        Domain: 'KO', Type: 'bef', Command: 'TimerReset', Value1: 2,
      });
    }
  }

  render() {
    const x = this.props.timer ? this.props.timer.find((y) => y.Name === 'Nachspielzeit') : null;
    const y = this.props.timerObjects ? this.props.timerObjects.find((z) => z.ID === 2) : null;

    const active = y ? y.Status : false;

    let item = <div>kein Timer gefunden</div>;
    if (x) {
      item = (
        <Zeit2
          timer={x}
          active={active}
          variable={x.Variable}
          wert={this.getVarWert(x.Variable)}
          click={this.handleClick.bind(this)}
          Name={x.Name}
          TimerNr={x.Nr}
          key={x.Nr}
          websocketSend={this.props.websocketSend.bind(this)}
        />
      );
    }

    return (
      <div>
        <div className="d-flex justify-content-center">
          {item}
        </div>
      </div>
    );
  }
}

ZeitenNachspielzeit.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object).isRequired,
  timer: PropTypes.arrayOf(PropTypes.object).isRequired,
  timerObjects: PropTypes.arrayOf(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

export default ZeitenNachspielzeit;
