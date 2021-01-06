import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { AiOutlineFileAdd } from 'react-icons/ai';
import Dropdown from './Dropdown';

class DlgFile extends Component {
  constructor(props) {
    super(props);
    this.state = {
      okdisabled: false,
      message: '',
      file: null,
      folder: 'Allgemein',
    };
  }

  nameChange(e) {
    const f = e.target.files[0];
    const r = new FileReader();

    r.onloadend = function read(e) { // eslint-disable-line no-shadow
      const contents = e.target.result;
      const b = { name: '', data: null, folder: '' };
      b.name = f.name;
      b.data = contents;
      b.folder = this.state.folder;
      this.setState({ file: b });
    }.bind(this);

    r.readAsArrayBuffer(f);
  }

  handleClick() {
    this.props.datei(this.state.file);
  }

  filter(e) {
    this.setState({ folder: e });
  }

  render() {
    let image = (
      <AiOutlineFileAdd />
    );
    if (this.props.reacticon !== '') {
      image = this.props.reacticon;
    }

    let cn = 'btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1';
    if (this.props.class !== '') {
      cn = this.props.class;
    }

    let folderWahl = null;
    if (this.props.folderWahl === 'yes') {
      folderWahl = (
        <div className="d-flex flex-row ml-1">
          <div className="ml-1 mr-1">Ziehl-Pfad:</div>
          <Dropdown
            values={['Allgemein', 'Mannschaften', 'Slideshow']}
            value={this.state.folder}
            wahl={this.filter.bind(this)}
          />
        </div>
      );
    }

    return (
      <div>
        <button
          type="button"
          className={cn}
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
        >
          {image}
        </button>

        <div className="modal fade" id={this.props.modalID} tabIndex="-1" role="dialog" aria-labelledby={this.props.modalID} aria-hidden="true">
          <div className="modal-dialog modal-dialog-centered" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title text-dark">{this.props.label1}</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body p-1">
                {folderWahl}
                <div className="m-1 p-1" style={{ overflow: 'visible' }}>
                  <input
                    type="file"
                    maxLength="40"
                    className="form-control p-0 m-1"
                    defaultValue={this.props.text}
                    accept={this.props.filter}
                    onChange={this.nameChange.bind(this)}
                  />
                </div>
              </div>
              <div className="modal-footer">
                <div className="text-dark">{this.state.message}</div>
                <button
                  type="button"
                  className="btn btn-primary"
                  data-dismiss="modal"
                  onClick={this.handleClick.bind(this)}
                  disabled={this.state.okdisabled}
                >
                  OK
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgFile.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  reacticon: PropTypes.string,
  class: PropTypes.string,
  toolTip: PropTypes.string,
  text: PropTypes.string,
  filter: PropTypes.string,
  datei: PropTypes.func,
  folderWahl: PropTypes.bool,
};

DlgFile.defaultProps = {
  modalID: 'filedlg',
  label1: 'Dateiauswahl',
  reacticon: '',
  class: '',
  toolTip: 'Dateiauswahl',
  text: '',
  filter: '',
  datei: () => {},
  folderWahl: 'no',
};

export default DlgFile;
