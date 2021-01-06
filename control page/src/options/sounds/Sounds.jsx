import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DlgSndSelection from '../../StdComponents/DlgSndSelection';

class Sounds extends Component {
  getPropValue(prop) {
    const x = this.props.options.find((y) => y.Prop === prop);
    return x ? x.Value : '';
  }

  handleSndSelection(property, e) {
    const value = e;
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: property,
      Value2: value,
    });
  }

  createList() {
    const list = [];
    for (let i = 1; i < 9; i += 1) {
      const snr = `Ton${i}`;
      list.push(
        <div key={i} className="m-1 p-1 d-flex flex-row align-items-center border border-dark">
          <div className="pr-1">{`${i}:`}</div>

          <img
            src={`./../../../sounds/${this.getPropValue(snr)}`}
            alt=""
            style={{ maxHeight: '3rem', maxWidth: '3rem', align: 'middle' }}
          />

          <div className="pl-1 pr-1 pb-1 m-1 flex-grow-1">
            {this.getPropValue(snr)}
          </div>

          <DlgSndSelection
            label1="Sound Selection:"
            data-toggle="tooltip"
            data-placement="right"
            toolTip="Audiodatei wählen"
            modalID={`Modal-S4${i}`}
            values={this.props.soundList}
            selection={this.handleSndSelection.bind(this, snr)}
          />
        </div>,
      );
    }

    return list;
  }

  render() {
    return (
      <div className="p-1 ml-2">
        <u>Sounds für die 8 Schnelltasten</u>
        {this.createList()}
      </div>
    );
  }
}

Sounds.propTypes = {
  options: PropTypes.oneOfType(PropTypes.object),
  soundList: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
};

Sounds.defaultProps = {
  options: [],
  soundList: [],
  websocketSend: () => {},
};

export default Sounds;
