import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Laufschrift extends Component {
  handleTextChange(e) {
    this.props.websocketSend({ Type: 'bef', Command: 'SetTicker', Value1: e.target.value });
  }

  render() {
    let text = 'freeScoreBoard.org';
    if (this.props.textvariablen.length > 0) {
      text = this.props.textvariablen.find((x) => x.ID === 'S34').Wert;
    }

    const ib = (
      <input
        className="form-control pl-1"
        type="text"
        defaultValue={text}
        onChange={this.handleTextChange.bind(this)}
      />
    );

    return (
      <div className="">
        {ib}
      </div>
    );
  }
}

Laufschrift.propTypes = {
  textvariablen: PropTypes.arrayOf(PropTypes.object),
  websocketSend: PropTypes.func.isRequired,
};

Laufschrift.defaultProps = {
  textvariablen: [],
};

export default Laufschrift;
