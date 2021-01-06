import React, { Component } from 'react';
import PropTypes from 'prop-types';

class DlgPicSelector extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleclick(bild) {
    if (bild === '[kein Bild]') {
      this.props.wahl('');
    } else {
      this.props.wahl(bild);
    }
  }

  render() {
    const items = this.props.werte.map((i) => { // eslint-disable-line arrow-body-style
      return (
        <button
          type="button"
          className="dropdown-item"
          key={i}
          data-dismiss="modal"
          onClick={this.handleclick.bind(this, i)}
        >
          <div className="m-1 p-1 d-flex flex-row align-items-baseline border border-dark">
            <img src={`./../../pictures/${i}`} alt="" style={{ maxHeight: '10rem', maxWidth: '10rem', align: 'middle' }} />
            <div className="pl-1">{i}</div>
          </div>
        </button>
      );
    });

    return (
      <div>
        <button
          type="button"
          className="btn btn-outline-secondary p-0 pl-1 pr-1 pb-0"
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
          disabled={this.props.disabled}
        >
          {this.props.iconAltText}
          {/* <img src={require('../Icons/' + this.props.icon)} alt={this.props.iconAltText} /> */}
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
                <div className="" style={{ overflow: 'scroll', height: '50vh' }}>
                  {items}
                </div>
              </div>

              <div className="modal-footer">
                <div className="text-dark">{this.state.meldung}</div>
              </div>

            </div>
          </div>
        </div>
      </div>
    );
  }
}

DlgPicSelector.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  toolTip: PropTypes.string,
  iconAltText: PropTypes.string,
  disabled: PropTypes.bool,
  werte: PropTypes.arrayOf(PropTypes.string),
  wahl: PropTypes.func,
};

DlgPicSelector.defaultProps = {
  modalID: 'filedlg',
  label1: 'Bildauswahl',
  toolTip: '',
  iconAltText: 'B',
  disabled: false,
  werte: [],
  wahl: () => {},
};

export default DlgPicSelector;
