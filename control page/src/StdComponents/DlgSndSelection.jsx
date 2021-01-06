import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { MdAudiotrack } from 'react-icons/md';

class DlgSndSelection extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleclick(pic) {
    if (pic === '[kein Ton]') {
      this.props.selection('');
    } else {
      this.props.selection(pic);
    }
  }

  render() {
    let items = null;
    if (this.props.values) {
      items = this.props.values.map((i) => { // eslint-disable-line arrow-body-style
        return (
          <button
            type="button"
            className="m1 btn btn-outline-dark"
            key={i}
            data-dismiss="modal"
            onClick={this.handleclick.bind(this, i)}
          >
            <div className="p-1">
              <div className="pl-1">
                {i}
              </div>
            </div>
          </button>
        );
      });
    }

    return (
      <div>
        <button
          type="button"
          className="btn btn-outline-secondary btn-icon p-0 pl-1 pr-1 pb-1 ml-2 mr-2"
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
        >
          <MdAudiotrack size="1.2em" />
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
                <div className="d-flex flex-column" style={{ overflow: 'scroll', height: '50vh' }}>
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

DlgSndSelection.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  toolTip: PropTypes.string,
  values: PropTypes.arrayOf(PropTypes.string),
  selection: PropTypes.func,
};

DlgSndSelection.defaultProps = {
  modalID: 'filedlg',
  label1: 'Soundauswahl',
  toolTip: '',
  values: [],
  selection: () => {},
};

export default DlgSndSelection;
