import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { IoMdDownload } from 'react-icons/io';

class DownloadJSon extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleClick() {
    if (this.props.format === 'json') {
      this.exportJSonFile(this.props.json, this.props.filename);
    } else {
      this.exportCSVFile(this.props.json, this.props.filename);
    }
  }

  exportJSonFile(jsonObject, filename) {
    // Convert Object to JSON

    const contentType = 'application/json;charset=utf-8;';

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      const blob = new Blob([decodeURIComponent(encodeURI(jsonObject))], { type: contentType });
      navigator.msSaveOrOpenBlob(blob, filename);
    } else {
      const a = document.createElement('a');

      a.download = filename;
      a.href = `data:${contentType},${encodeURIComponent(jsonObject)}`;
      a.target = '_blank';
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }

  exportCSVFile(items, filename) {
    // Convert Object to JSON
    const jsonObject = items;

    const csv = this.convertToCSV(jsonObject);

    const contentType = 'application/json;charset=utf-8;';

    if (window.navigator && window.navigator.msSaveOrOpenBlob) {
      const blob = new Blob([decodeURIComponent(encodeURI(csv))], { type: contentType });
      navigator.msSaveOrOpenBlob(blob, filename);
    } else {
      const a = document.createElement('a');

      a.download = filename;
      a.href = `data:${contentType},${encodeURIComponent(csv)}`;
      a.target = '_blank';
      document.body.appendChild(a);
      a.click();
      document.body.removeChild(a);
    }
  }

  convertToCSV(objArray) {
    const array = typeof objArray !== 'object' ? JSON.parse(objArray) : objArray;
    let str = '';

    // header
    let hline = '';
    const keys = Object.keys(array[0]);
    for (let i = 0; i < keys.length; i += 1) {
      if (hline !== '') {
        hline += ';';
      }
      const wert = keys[i];
      hline += wert;
    }

    str += `${hline}\r\n`;

    // items
    for (let i = 0; i < array.length; i += 1) {
      let line = '';
      for (let j = 0; j < keys.length; j += 1) {
        if (line !== '') {
          line += ';';
        }

        const wert = array[i][keys[j]];
        line += wert || '';
      }

      str += `${line}\r\n`;
    }

    return str;
  }

  render() {
    const cn = this.props.btnclassname ? this.props.btnclassname : 'btn btn-outline-secondary btn-icon btn-sm';

    return (
      <div>
        <button
          type="button"
          className={cn}
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          onClick={this.handleClick.bind(this)}
        >
          <IoMdDownload size="1.2em" />
        </button>
      </div>
    );
  }
}

DownloadJSon.propTypes = {
  json: PropTypes.string,
  filename: PropTypes.string,
  toolTip: PropTypes.string,
  btnclassname: PropTypes.string,
  format: PropTypes.string,
};

DownloadJSon.defaultProps = {
  json: '',
  filename: '',
  toolTip: '',
  btnclassname: '',
  format: '',
};

export default DownloadJSon;
