import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Tablerow extends Component {
  constructor() {
    super();
    this.state = {
    };
  }

  getDepEventName(e) {
    const t = this.props.evtType.find((x) => x.Nummer === e);
    return t ? t.Name : null;
  }

  setSelected() {
    let r = false;
    if (this.props.chkid) {
      if (this.props.chkid.length > 0) {
        if (this.props.chkid.includes(this.props.row.ID)) {
          r = true;
        }
      }
    }
    return r;
  }

  render() {
    // Datenspalten auflisten
    const newArr = [];
    // const keys = Object.keys(this.props.row)
    // if (this.props.cols.length > 0){
    //   let fieldlist = this.props.cols.map(x => x.Column)
    //   for(const key of keys){
    //     if (fieldlist.includes(key)){
    //       newArr.push(this.props.row[key]);
    //     }
    //   }
    // }
    if (this.props.cols) {
      if (this.props.cols.length > 0) {
        let fieldlist = this.props.cols;
        if (typeof this.props.cols[0] === 'object') {
          fieldlist = this.props.cols.map((x) => x.Column);
        }
        for (let i = 0; i < fieldlist.length; i += 1) {
          newArr.push(this.props.row[fieldlist[i]]);
        }
      }
    } else {
      const f = Object.keys(this.props.row);
      for (let i = 0; i < f.length; i += 1) {
        newArr.push(this.props.row[f[i]]);
      }
    }

    const selected = this.setSelected();
    const co2 = newArr.map((col2, index) => { // eslint-disable-line arrow-body-style
      let c2 = col2;
      if (typeof col2 === 'boolean') {
        c2 = col2 ? 'ja' : 'nein';
      }
      return (
        // eslint-disable-next-line react/no-array-index-key
        <td className="text-dark pt-0 pb-0" key={index}>
          {c2}
        </td>
      );
    });

    let chktype = 'radio';
    if (this.props.chktype) {
      chktype = this.props.chktype;
    }

    // console.log(this.props);
    const chk = this.props.chk !== '' ? (
      <td
        className="pl-1 pr-1 pt-0 pb-0"
        key={co2[0]}
      >
        <input
          type={chktype}
          name={this.props.radiogrp}
          value={co2[0]}
          onClick={this.props.rowClick ? this.props.rowClick.bind(this, this.props.row.ID) : null}
          key={co2[0]}
          checked={selected}
        />
      </td>
    ) : null;

    // console.log(this.props.row);

    return (
      <tr className="pt-0 pb-0" key={co2}>
        {chk}
        {co2}
      </tr>
    );
  }
}

Tablerow.propTypes = {
  row: PropTypes.arrayOf(PropTypes.object),
  cols: PropTypes.arrayOf(PropTypes.string),
  chkid: PropTypes.arrayOf(PropTypes.object),
  chk: PropTypes.string,
  chktype: PropTypes.string,
  radiogrp: PropTypes.string,
  evtType: PropTypes.string,
  rowClick: PropTypes.func,
};

Tablerow.defaultProps = {
  row: [],
  cols: [],
  chkid: [],
  chk: '',
  chktype: '',
  radiogrp: '',
  evtType: '',
  rowClick: () => {},
};

export default Tablerow;
