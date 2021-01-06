import React, { Component } from 'react';
import PropTypes from 'prop-types';
import Tablerow from './Tablerow';
// import JsonTable from 'react-json-table';

// import JsonTable from 'ts-react-json-table';

class Table extends Component {
  constructor() {
    super();
    this.state = {
    };
  }

  fillColumnNames() {
    const hlabels = [];
    for (let i = 0; i < this.props.daten[0]; i += 1) {
      hlabels.push(this.props.daten[0][i]);
    }

    return hlabels;
  }

  rowClick(ID, e) {
    if (this.props.rowClick) {
      this.props.rowClick(ID, e);
    }
  }

  render() {
    // console.log(this.props.daten);

    // Header
    let co = <div>keine Daten</div>;
    let selAll = '';
    if (this.props.chktype === 'checkbox' && this.props.selAllClick !== null) {
      let allChecked = false;
      if (this.props.daten.length === this.props.chkid.length && this.props.daten.length > 0) {
        allChecked = true;
      }
      selAll = (
        <th className="pt-0 pb-0">
          <input
            type="checkbox"
            checked={allChecked}
            indeterminate={this.props.selAllIndeterminate}
            onClick={this.props.selAllClick}
          />
        </th>
      );
    }

    let h = this.props.cols;
    if (h.length > 0) {
      co = h
        .map((col) => { // eslint-disable-line arrow-body-style
          return (
            <th className="pt-0 pb-0" key={col.Column}>{col.Label}</th>
          );
        });
    } else if (this.props.daten) {
      if (this.props.daten.length > 0) {
        h = Object.keys(this.props.daten[0]);
        co = h
          .map((col) => { // eslint-disable-line arrow-body-style
            return (
              <th className="pt-0 pb-0" key={col}>{col}</th>
            );
          });
      }
    }

    // check if selection column must be displayed
    const chk = this.props.chk && selAll === '' ? <th label="header" className="pt-0 pb-0" /> : null;

    // items

    let dz = '';
    if (this.props.daten) {
      dz = this.props.daten.map((ite) => { // eslint-disable-line arrow-body-style
        return (
          <Tablerow
            cols={h}
            key={ite.key}
            row={ite}
            chk={this.props.chk}
            chkid={this.props.chkid ? this.props.chkid : ['']}
            rowClick={this.props.rowClick ? this.props.rowClick.bind(this) : null}
            chktype={this.props.chktype}
            radiogrp={this.props.radiogrp}
          />
        );
      });
    }

    return (
      <div className="p-0 border border-dark" style={{ height: this.props.height, overflowY: 'auto' }}>
        <table className="table table-sm p-0 m-0 bs-0">
          <thead className="thead-light">
            <tr className="d-table-row">
              {selAll}
              {chk}
              {co}
            </tr>
          </thead>
          <tbody style={{ overflow: 'auto' }}>
            {dz}
          </tbody>
        </table>
      </div>
    );
  }
}

Table.propTypes = {
  daten: PropTypes.arrayOf(PropTypes.object),
  cols: PropTypes.arrayOf(PropTypes.string),
  chkid: PropTypes.arrayOf(PropTypes.object),
  chk: PropTypes.string,
  chktype: PropTypes.string,
  radiogrp: PropTypes.string,
  height: PropTypes.string,
  rowClick: PropTypes.func,
  selAllClick: PropTypes.func,
  selAllIndeterminate: PropTypes.bool,
};

Table.defaultProps = {
  daten: [],
  cols: [],
  chkid: [],
  chk: '',
  chktype: 'radio',
  radiogrp: '',
  height: '',
  rowClick: () => {},
  selAllClick: null,
  selAllIndeterminate: false,
};

export default Table;
