import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoGear } from 'react-icons/go';
import Checkbox from './Checkbox';

class DlgFensterwahl extends Component {
  constructor(props) {
    super(props);
    this.state = {
      modalVisible: false,
    };

    this.tooglemodal = this.tooglemodal.bind(this);
  }

  tooglemodal() {
    this.setState((prevState) => ({
      modalVisible: !prevState.modalVisible,
    }));
  }

  handleclick(e) {
    const fen = this.props.werte;
    const i = fen.findIndex((x) => x.Name === e);

    fen[i].sichtbar = !fen[i].sichtbar;
  }

  render() {
    const items = this.props.werte.map((i) => { // eslint-disable-line arrow-body-style
      return (
        <Checkbox
          id={i.Name}
          key={i.Name}
          label={i.Name}
          checked={i.sichtbar}
          handleCheckboxChange={this.handleclick.bind(this, i.Name)}
        />
      );
    });

    const modalID = 'Berichtssetup';

    return (
      <div>
        <button
          type="button"
          className="btn btn-outline-secondary btn-sm mr-1"
          data-placement="right"
          title="Berichtssetup"
          label="Berichtssetup"
          data-toggle="modal"
          data-target={`#${modalID}`}
        >
          <GoGear size="1.2em" />
        </button>

        <div className="modal fade" id={modalID} tabIndex="-1" role="dialog" aria-labelledby={modalID} aria-hidden="true">
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

DlgFensterwahl.propTypes = {
  werte: PropTypes.arrayOf(PropTypes.object).isRequired,
  label1: PropTypes.string,
};

DlgFensterwahl.defaultProps = {
  label1: 'kein Titel',
};

export default DlgFensterwahl;
