import React, { Component } from 'react';
import PropTypes from 'prop-types';
import DropdownWithID from '../../StdComponents/DropdownWithID';

class Display extends Component {
  setPropValue(prop, e) {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: prop,
      Value2: e.target.value,
    });
  }

  setPropValueDirect(prop, e) {
    const value = e;
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: prop,
      Value2: value,
    });
  }

  getPropValue(prop) {
    const x = this.props.options.find((y) => y.Prop === prop);
    return x ? x.Value : '';
  }

  render() {
    let w = 'off';
    let bw = true;
    if (this.getPropValue('DisplayScreenFull') === 'off') {
      w = 'on';
      bw = false;
    }

    let sa = true;
    let sw = 'off';
    if (this.getPropValue('DisplayAtStartup') === 'off') {
      sw = 'on';
      sa = false;
    }

    const pl = [];
    for (let i = 0; i < this.props.PageSets.length; i += 1) {
      pl.push({
        ID: this.props.PageSets[i].ID.toString(),
        Text: this.props.PageSets[i].PageSetName,
      });
    }

    return (
      <div className="p-1 ml-1">
        <u>Anzeige</u>

        <div className="d-flex flex-row p-2">
          <div className="mr-2">
            Start Anzeigeprofil:
          </div>
          <DropdownWithID
            className="ml-2"
            values={pl}
            value={this.getPropValue('StartPageSet')}
            wahl={this.setPropValueDirect.bind(this, 'StartPageSet')}
          />
        </div>

        <div className="mt-2">lokales Anzeigefenster:</div>

        <div className="d-flex flex-row p-2">
          <div className="pl-1" style={{ width: '100px' }}>
            Pos-Y:
          </div>
          <input
            title="Abstand von Oben in Pixel"
            type="number"
            className=""
            style={{ width: '60px' }}
            min="0"
            max="10000"
            value={this.getPropValue('DisplayScreenTop')}
            onChange={this.setPropValue.bind(this, 'DisplayScreenTop')}
          />

          <div className="pl-2" style={{ width: '100px' }}>
            Pos-X:
          </div>
          <input
            title="Abstand von Links in Pixel"
            type="number"
            className=""
            style={{ width: '60px' }}
            min="0"
            max="10000"
            value={this.getPropValue('DisplayScreenLeft')}
            onChange={this.setPropValue.bind(this, 'DisplayScreenLeft')}
          />
        </div>

        <div className="d-flex flex-row p-2">
          <div className="pl-1" style={{ width: '100px' }}>
            Größe-Y:
          </div>
          <input
            title="Höhe in Pixel"
            type="number"
            className=""
            style={{ width: '60px' }}
            min="0"
            max="10000"
            value={this.getPropValue('DisplayScreenHeight')}
            onChange={this.setPropValue.bind(this, 'DisplayScreenHeight')}
          />

          <div className="pl-2" style={{ width: '100px' }}>
            Größe-X:
          </div>
          <input
            title="Breite in Pixel"
            type="number"
            className=""
            style={{ width: '60px' }}
            min="0"
            max="10000"
            value={this.getPropValue('DisplayScreenWidth')}
            onChange={this.setPropValue.bind(this, 'DisplayScreenWidth')}
          />
        </div>

        <div className="d-flex flex-row p-2">
          <div className="pl-1" style={{ width: '100px' }}>
            Monitor Nr.:
          </div>
          <input
            title="Monitorl"
            type="number"
            className="forms-control"
            style={{ width: '60px' }}
            min="0"
            max="10"
            value={this.getPropValue('DisplayScreenNumber')}
            onChange={this.setPropValue.bind(this, 'DisplayScreenNumber')}
          />

          <div className="pl-2" style={{ width: '100px' }}>
            Vollbild:
          </div>
          <label htmlFor="Vollbild" className="switch pl-2 text-center">
            <input type="checkbox" id="Vollbild" checked={bw} value={w} onChange={this.setPropValue.bind(this, 'DisplayScreenFull')} />
            <span className="slider round" />
          </label>
        </div>

        <div className="d-flex flex-row p-2">
          <div className="pl-1 mr-2">
            bei Programmstart anzeigen:
          </div>
          <label htmlFor="startanzeige" className="switch text-center">
            <input type="checkbox" id="startanzeige" checked={sa} value={sw} onChange={this.setPropValue.bind(this, 'DisplayAtStartup')} />
            <span className="slider round" />
          </label>
        </div>
      </div>
    );
  }
}

Display.propTypes = {
  options: PropTypes.oneOfType(PropTypes.object),
  PageSets: PropTypes.oneOfType(PropTypes.object),
  websocketSend: PropTypes.func,
};

Display.defaultProps = {
  options: [],
  PageSets: [],
  websocketSend: () => {},
};

export default Display;
