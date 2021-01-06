import React, { Component }  from 'react';

class TableRow extends Component {
  render() {
    
    let row = '';
    if (this.props.row) {
      const keys = Object.keys(this.props.row);
      row = keys.map((col) => { // eslint-disable-line arrow-body-style
        return (
          <td key={col}>{this.props.row[col]}</td>
        );
      });
    }

    return (
      <tr>
        {row}
      </tr>
    );
  }
}

export default TableRow;
