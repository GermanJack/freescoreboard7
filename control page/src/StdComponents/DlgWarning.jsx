import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { IoIosWarning } from 'react-icons/io';

class DlgWarning extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleClick() {
    this.props.name('OK');
  }

  render() {
    const cn = this.props.btnclassname ? this.props.btnclassname : 'btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1';
    let defaulticon = '';
    if (this.props.icon !== '') {
      defaulticon = (
        <IoIosWarning />
      );
    }

    let ico = this.props.icon ? defaulticon : this.props.reacticon;
    if (this.props.icon === '' && this.props.reacticon === '') {
      ico = this.props.btntext;
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
          {ico}
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
              <div className="modal-footer">
                <button type="button" className="btn btn-secondary" data-dismiss="modal">Abbruch</button>
                <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={this.handleClick.bind(this)}>OK</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgWarning.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  toolTip: PropTypes.string,
  reacticon: PropTypes.string,
  btnclassname: PropTypes.string,
  icon: PropTypes.string,
  name: PropTypes.func,
  btntext: PropTypes.string,
};

DlgWarning.defaultProps = {
  modalID: 'filedlg',
  label1: 'Tabelle',
  toolTip: '',
  reacticon: '',
  btnclassname: '',
  icon: '',
  name: () => {},
  btntext: 'Achtung',
};

export default DlgWarning;
