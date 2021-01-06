import React, { Component } from 'react';
import PropTypes from 'prop-types';

class DlgMessage extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  okClick() {
    this.props.messageShow = false;
  }

  render() {
    if (!this.props.messageShow) {
      return null;
    }

    return (
      <div className="modal fade" id={this.props.modalID} tabIndex="-1" role="dialog" aria-labelledby={this.props.modalID} aria-hidden="true">
      </div>
    );
  }
}

DlgMessage.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  message: PropTypes.string,
  messageShow: PropTypes.bool,
};

DlgMessage.defaultProps = {
  modalID: 'filedlg',
  label1: 'Message',
  message: '',
  messageShow: false,
};

export default DlgMessage;
