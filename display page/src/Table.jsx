import React, { Component }  from 'react';
import TableRow from './TableRow';
import './TStyleEinfachGruen.css';
import './TStyleEinfachBlau.css';
import './TStyleSchwarz.css';
import './TStyleWeis.css';
import './TStyleStandard.css';
import './TStyleZeilenBlau.css';
import './TStyleZeilenGruen.css';

class Table extends Component {
  constructor (props) {
    super(props);
    this.state = {
    };
  };

  render() {
    let head = <div></div>;
    if (this.props.daten) {
      if (this.props.daten.length > 0) {
        const keys = Object.keys(this.props.daten[0]);
        head = keys.map((col) => { // eslint-disable-line arrow-body-style
          return (
            <th key={col}>{col}</th>
          );
        });
      }
    }

    let body = '';
    if (this.props.daten) {
      body = this.props.daten.map((ite) => { // eslint-disable-line arrow-body-style
        return (
          <TableRow
            key={ite.key}
            row={ite}
          />
        );
      });
    }

    return (
      <div>
        <table className={this.props.tablestyle} >
          <thead>
            <tr>
              {head}
            </tr>
          </thead>
          <tbody>
            {body}
          </tbody>
        </table>
      </div>
    );
  }
}

export default Table;