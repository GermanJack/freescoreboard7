/* eslint-disable jsx-a11y/label-has-associated-control */
import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { BiSort } from 'react-icons/bi';
import { MdLowPriority } from 'react-icons/md';
import Table from '../../StdComponents/Table';

class Turnier extends Component {
  constructor(props) {
    super(props);
    this.state = {
      chkTabsort: 0,

    };
  }

  getPropValue(prop) {
    const x = this.props.options.find((y) => y.Prop === prop);
    return x ? x.Value : '';
  }

  setPropValue(property, e) {
    let v = e.target.value;
    if (v % 1 !== 0) {
      v = Math.round(v);
    }
    this.props.websocketSend({
      Type: 'bef',
      Command: 'SetOptValue',
      Value1: property,
      Value2: v,
    });
  }

  tabsortRowClick(id) {
    const ml = this.props.tabsort;
    const mi = ml.findIndex((x) => x.ID === id);
    const m = this.props.tabsort[mi];
    this.setState({ chkTabsort: m });
  }

  changeSort() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'ChangeTabSortOrder',
      Value1: this.state.chkTabsort.ID,
    });
  }

  changePrio() {
    this.props.websocketSend({
      Type: 'bef',
      Command: 'LowerTabSortPrio',
      Value1: this.state.chkTabsort.ID,
    });
  }

  render() {
    const spkt = this.getPropValue('Siegerpunkte');
    const vpkt = this.getPropValue('Verliererpunkte');
    const upkt = this.getPropValue('Unentschiedenpunkte');

    let disable = true;
    if (this.state.chkTabsort !== 0) {
      disable = false;
    }

    return (
      <div className="p-1 ml-2">
        <u>Turnieroptionen</u>

        <div>Punkte:</div>
        <div className="d-flex flex-column">
          <div className="form-group row">
            <label htmlFor="besch" className="col-sm-7 col-form-label">Siegerpunkte</label>
            <div className="col-sm-4">
              <input
                className="form-control"
                type="number"
                min="0"
                max="9"
                step="1"
                id="SPunkte"
                value={spkt}
                spellCheck="false"
                onChange={this.setPropValue.bind(this, 'Siegerpunkte')}
                title="Punkte die der Siegermannschaft zugerechnet werden"
              />
            </div>
          </div>

          <div className="form-group row">
            <label htmlFor="besch" className="col-sm-7 col-form-label">Verliererpunkte</label>
            <div className="col-sm-4">
              <input
                className="form-control"
                type="number"
                min="0"
                max="9"
                step="1"
                id="SPunkte"
                value={vpkt}
                spellCheck="false"
                onChange={this.setPropValue.bind(this, 'Verliererpunkte')}
                title="Punkte die der Verlierermannschaft zugerechnet werden"
              />
            </div>
          </div>

          <div className="form-group row">
            <label htmlFor="besch" className="col-sm-7 col-form-label">Unentschiedenpunkte</label>
            <div className="col-sm-4">
              <input
                className="form-control"
                type="number"
                min="0"
                max="9"
                step="1"
                id="SPunkte"
                value={upkt}
                spellCheck="false"
                onChange={this.setPropValue.bind(this, 'Unentschiedenpunkte')}
                title="Punkte die bei Unentschieden beiden Mannschaften zugerechnet werden"
              />
            </div>
          </div>
        </div>

        <div>Tabellensortierung:</div>
        <div className="d-flex flex-row">
          <div className="d-flex flex-column">
            <Table
              daten={this.props.tabsort}
              cols={[
                { Column: 'Feld', Label: 'Feldname' },
                { Column: 'Prio', Label: 'Priorität' },
                { Column: 'absteigend', Label: 'absteigend' },
              ]}
              chkid={this.state.chkTabsort ? [this.state.chkTabsort.ID] : []}
              chk="Selected"
              radiogrp="tabsort"
              height={300}
              rowClick={this.tabsortRowClick.bind(this)}
            />
          </div>
          <div className="d-flex flex-column">
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 mt-4 ml-2 mr-2"
              title="ab-/aufsteigend"
              onClick={this.changeSort.bind(this)}
              disabled={disable}
            >
              <BiSort size="1.2rem" />
            </button>
            <button
              type="button"
              className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 mt-1 ml-2 mr-2"
              title="geringere Priorität"
              onClick={this.changePrio.bind(this)}
              disabled={disable}
            >
              <MdLowPriority size="1.2rem" />
            </button>
          </div>
        </div>

      </div>
    );
  }
}

Turnier.propTypes = {
  options: PropTypes.OfType(PropTypes.object),
  tabsort: PropTypes.OfType(PropTypes.object),
  websocketSend: PropTypes.func.isRequired,
};

Turnier.defaultProps = {
  options: [],
  tabsort: [],
};

export default Turnier;
