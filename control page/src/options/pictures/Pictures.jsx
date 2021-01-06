import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DlgPicSelection from '../../StdComponents/DlgPicSelection';

class Pictures extends Component {
  getPropValue(prop) {
    const x = this.props.options.find((y) => y.Prop === prop);
    return x ? x.Value : '';
  }

  handlePicSelection(property, e) {
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
      const bnr = `SlideshowBild${i}`;
      list.push(
        <div key={i} className="m-1 p-1 d-flex flex-row align-items-center border border-dark">
          <div className="pr-1">{`${i}:`}</div>

          <img
            src={`./../../../pictures/${this.getPropValue(bnr)}`}
            alt=""
            style={{ maxHeight: '3rem', maxWidth: '3rem', align: 'middle' }}
          />

          <div className="pl-1 pr-1 pb-1 m-1 flex-grow-1">
            {this.getPropValue(bnr)}
          </div>

          <DlgPicSelection
            label1="Bildwahl:"
            text=""
            class="btn btn-outline-secondary btn-icon btn-sm"
            data-toggle="tooltip"
            data-placement="right"
            toolTip="Bild wählen"
            modalID={`Modal-S4${i}`}
            values={this.props.picList}
            selection={this.handlePicSelection.bind(this, bnr)}
          />
        </div>,
      );
    }

    return list;
  }

  createPaintballList() {
    const list = [];
    for (let i = 1; i < 3; i += 1) {
      const bnr = `Paintball Bild${i}`;
      list.push(
        <div key={i} className="m-1 p-1 d-flex flex-row align-items-center border border-dark">
          <div className="pr-1">{`${i}:`}</div>

          <img
            src={`./../../../pictures/${this.getPropValue(bnr)}`}
            alt=""
            style={{ maxHeight: '3rem', maxWidth: '3rem', align: 'middle' }}
          />

          <div className="pl-1 pr-1 pb-1 m-1 flex-grow-1">
            {this.getPropValue(bnr)}
          </div>

          <DlgPicSelection
            label1="Bildwahl:"
            text=""
            class="btn btn-outline-secondary btn-icon btn-sm"
            data-toggle="tooltip"
            data-placement="right"
            toolTip="Bild wählen"
            modalID={`Modal-S5${i}`}
            values={this.props.picList}
            selection={this.handlePicSelection.bind(this, bnr)}
          />
        </div>,
      );
    }

    return list;
  }

  createSpielrichtungList() {
    const list = [];
    for (let i = 1; i < 3; i += 1) {
      let bnr = 'SpielrichtungLinks';
      let tx = 'Links';
      if (i === 2) {
        bnr = 'SpielrichtungRechts';
        tx = 'rechts';
      }
      list.push(
        <div key={i} className="m-1 p-1 d-flex flex-row align-items-center border border-dark">
          <div className="pr-1">{`${tx}:`}</div>

          <img
            src={`./../../../pictures/${this.getPropValue(bnr)}`}
            alt=""
            style={{ maxHeight: '3rem', maxWidth: '3rem', align: 'middle' }}
          />

          <div className="pl-1 pr-1 pb-1 m-1 flex-grow-1">
            {this.getPropValue(bnr)}
          </div>

          <DlgPicSelection
            label1="Bildwahl:"
            text=""
            class="btn btn-outline-secondary btn-icon btn-sm"
            data-toggle="tooltip"
            data-placement="right"
            toolTip="Bild wählen"
            modalID={`Modal-SR5${i}`}
            values={this.props.picList}
            selection={this.handlePicSelection.bind(this, bnr)}
          />
        </div>,
      );
    }

    return list;
  }

  render() {
    return (
      <div className="p-1 ml-2">
        <u>Bilder für die 8 Schnelltasten</u>
        {this.createList()}
        <u>Paintball Bilder</u>
        {this.createPaintballList()}
        <u>Spielrichtung Bilder</u>
        {this.createSpielrichtungList()}
      </div>
    );
  }
}

Pictures.propTypes = {
  options: PropTypes.OfType(PropTypes.object),
  picList: PropTypes.OfType(PropTypes.object),
  websocketSend: PropTypes.func,
};

Pictures.defaultProps = {
  options: [],
  picList: [],
  websocketSend: () => {},
};

export default Pictures;
