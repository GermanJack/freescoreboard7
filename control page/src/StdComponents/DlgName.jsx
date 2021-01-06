import React, { Component } from 'react';
import PropTypes from 'prop-types';

class DlgName extends Component {
  constructor(props) {
    super(props);
    this.state = {
      name: this.props.text,
      okdisabled: true,
      message: 'no Input / keine Eingabe',
    };
  }

  nameChange(e) {
    this.setState({ name: e.target.value }, () => this.checkStateName());
  }

  checkStateName() {
    const regex = this.props.allowSpace ? '[^A-Za-z0-9_ öÖäÄüÜß]' : '[^A-Za-z0-9_öÖäÄüÜß]';
    const re = new RegExp(regex);
    if (re.test(this.state.name)) {
      this.setState({ okdisabled: true, message: 'nicht erlaubte Zeichen' });
      return;
    }

    const p = this.props.values.indexOf(this.state.name, 0);
    if (p > -1) {
      this.setState({ okdisabled: true, message: 'Element schon vorhanden' });
      return;
    }

    if (this.state.name.length === 0) {
      this.setState({ okdisabled: true, message: 'keine Eingabe' });
      return;
    }

    this.setState({ okdisabled: false, message: 'alles gut' });
  }

  handleClick() {
    this.props.name(this.state.name);
    this.setState({ name: '' });
    this.setState({ okdisabled: true, message: 'no Input / keine Eingabe' });
  }

  render() {
    let cn = 'btn btn-primary btn-icon border-dark p-0 mr-1';
    if (this.props.class !== '') {
      cn = this.props.class;
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
          {this.props.icon}
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
                <div className="form-group">
                  <input
                    type="text"
                    maxLength="20"
                    className="form-control"
                    value={this.state.name}
                    onChange={this.nameChange.bind(this)}
                  />
                </div>
              </div>
              <div className="modal-footer">
                <div className="text-dark">{this.state.message}</div>
                <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={this.handleClick.bind(this)} disabled={this.state.okdisabled}>OK</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgName.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  icon: PropTypes.string,
  class: PropTypes.string,
  toolTip: PropTypes.string,
  text: PropTypes.string,
  values: PropTypes.arrayOf(PropTypes.string),
  allowSpace: PropTypes.bool,
  name: PropTypes.func,
};

DlgName.defaultProps = {
  modalID: 'filedlg',
  label1: 'Dateiauswahl',
  icon: '',
  class: '',
  toolTip: 'Dateiauswahl',
  text: '',
  values: [],
  allowSpace: false,
  name: () => {},
};

export default DlgName;
