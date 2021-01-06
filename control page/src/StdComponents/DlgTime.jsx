import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { FaPencilAlt } from 'react-icons/fa';
import $ from 'jquery';
import MinuteTime from './MinuteTime';

class DlgTime extends Component {
  constructor(props) {
    super(props);
    this.state = {
      sec: '50',
      closemod: '',
    };
  }

  componentDidMount() {
    if (this.state.sec !== this.props.seconds) {
      this.setState({ sec: this.props.seconds });
    }

    $(`#${this.props.modalID}`).on('show.bs.modal', () => {
      this.onModalShow();
    });

    $(`#${this.props.modalID}`).on('hidden.bs.modal', () => {
      this.onModalClose();
    });
  }

  onModalShow() {
    this.setState({ closemod: '' });

    let sec = this.props.wert;

    if (this.props.displaymod === 'min') {
      const t = this.props.wert;

      const m = t.split(':')[0];
      const s = t.split(':')[1];
      sec = (parseInt(m, 10) * 60) + parseInt(s, 10);
    }

    this.setState({ sec });
  }

  onModalClose() {
    // how is modal closed
    // console.log(this.state.closemod);
    if (this.state.closemod === 'ok') {
      this.props.newTime(this.state.sec);
    }
  }

  timeChange(e) {
    this.setState({ sec: e });
  }

  handleClick() {
    this.setState({ closemod: 'ok' });
  }

  secChange(e) {
    this.setState({ sec: e.target.value });
  }

  render() {
    const edit = <FaPencilAlt />;

    let ze = (
      <div className="d-flex flex-row">
        <input
          title="Zeit:"
          type="number"
          className="mb-1 ml-2 mr-2"
          style={{ width: '100px' }}
          min="0"
          max="100000"
          value={this.state.sec}
          onChange={this.secChange.bind(this)}
        />
        <div>Sekunden</div>
      </div>
    );

    if (this.props.displaymod === 'min') {
      ze = (
        <MinuteTime
          seconds={this.state.sec}
          TimeSet={this.timeChange.bind(this)}
        />
      );
    }

    const cn = this.props.btnclassname ? this.props.btnclassname : 'btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1';

    return (
      <div>
        <button
          type="button"
          className={cn}
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
          disabled={this.props.disabled}
        >
          {/* <img src={require('../Icons/' + this.props.icon)} alt={this.props.iconAltText} /> */}
          {edit}
        </button>

        <div className="modal fade" id={this.props.modalID} tabIndex="-1" role="dialog" aria-labelledby={this.props.modalID} aria-hidden="true">
          <div className="modal-dialog modal-sm modal-dialog-centered" role="document">
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title text-dark">{this.props.label1}</h5>
                <button type="button" className="close" data-dismiss="modal" aria-label="Close">
                  <span aria-hidden="true">&times;</span>
                </button>
              </div>
              <div className="modal-body p-1">
                <div className="form-group">
                  {ze}
                </div>
              </div>
              <div className="modal-footer">
                <div className="text-dark">{this.state.message}</div>
                <button type="button" className="btn btn-primary" data-dismiss="modal" onClick={this.handleClick.bind(this)}>OK</button>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgTime.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  toolTip: PropTypes.string,
  displaymod: PropTypes.string,
  disabled: PropTypes.bool,
  btnclassname: PropTypes.string,
  wert: PropTypes.string,
  seconds: PropTypes.number,
  newTime: PropTypes.func,
};

DlgTime.defaultProps = {
  modalID: 'filedlg',
  label1: 'Tabelle',
  toolTip: '',
  displaymod: '',
  disabled: false,
  btnclassname: '',
  wert: '',
  seconds: 0,
  newTime: () => {},
};

export default DlgTime;
