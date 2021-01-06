import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Events extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  getPropValueBool(prop) {
    const x = this.props.options.find((y) => y.Prop === prop);
    const isTrueSet = x ? (x.Value === 'True') : false;
    return isTrueSet;
  }

  setEvent(ID, e) {
    if (ID) {
      this.props.websocketSend({ Command: 'SetEvent', Value1: ID, Value2: e.target.checked });
    }
  }

  setPropValue(property, e) {
    const value = e.target.checked;
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: property,
      Value2: value,
    });
  }

  delFreeEreig() {
    this.props.websocketSend({ Command: 'delFreeEreig' });
  }

  makeTorschuetze() {
    const z = (
      <td>
        <label htmlFor="TS" className="switch pl-2 ml-1 text-center">
          <input
            type="checkbox"
            id="TS"
            checked={this.getPropValueBool('Torschütze')}
            onChange={this.setPropValue.bind(this, 'Torschütze')}
          />
          <span className="slider round" />
        </label>
      </td>
    );
    return z;
  }

  makeFoulspieler() {
    const z = (
      <td>
        <label htmlFor="FS" className="switch pl-2 ml-1 text-center">
          <input
            type="checkbox"
            id="FS"
            checked={this.getPropValueBool('Foulspieler')}
            onChange={this.setPropValue.bind(this, 'Foulspieler')}
          />
          <span className="slider round" />
        </label>
      </td>
    );
    return z;
  }

  render() {
    const item = this.props.events.map((x) => {
      let l = <td />;
      let z = <td />;
      if (x.Nummer === '05 - Tor') {
        l = <td>Torschütze erfassen</td>;
        z = this.makeTorschuetze();
      }
      if (x.Nummer === '06 - Foul') {
        l = <td>Foulspieler erfassen</td>;
        z = this.makeFoulspieler();
      }

      const r = (
        <tr className="pb-1">
          <td>
            {x.Nummer}
          </td>
          <td className="border-right">
            <label htmlFor={x.ID} className="switch pl-2 pr-2 ml-1 text-center">
              <input
                type="checkbox"
                id={x.ID}
                checked={x.Log}
                onChange={this.setEvent.bind(this, x.ID)}
              />
              <span className="slider round" />
            </label>
          </td>
          {l}
          {z}
        </tr>
      );
      return r;
    });

    return (
      <div className="p-1 ml-2">
        <u>Ereignisprotokoll</u>

        <div className="p-1 ml-2">
          <button
            type="button"
            className="btn btn-primary border border-dark"
            onClick={this.delFreeEreig.bind(this)}
          >
            Freispielereignisse löschen
          </button>
        </div>

        <div className="p-0 border border-dark" style={{ overflowY: 'auto' }}>
          <table className="table table-sm p-0 m-0 bs-0">
            <thead className="thead-light">
              <tr className="pb-1">
                <td>Ereigniss</td>
                <td className="border-right">Protokoll ein</td>
                <td>Zusatzeinstellung</td>
                <td>ja/nein</td>
              </tr>
            </thead>
            <tbody style={{ overflow: 'auto' }}>
              {item}
            </tbody>
          </table>
        </div>
      </div>
    );
  }
}

Events.propTypes = {
  events: PropTypes.oneOfType(PropTypes.object).isRequired,
  options: PropTypes.oneOfType(PropTypes.object).isRequired,
  websocketSend: PropTypes.func.isRequired,
};

Events.defaultProps = {
};

export default Events;
