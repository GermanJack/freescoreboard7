import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { GoGear } from 'react-icons/go';
import Checkbox from './Checkbox';

class DlgFeldwahl extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleclick(e) {
    const fe = this.props.werte;
    const i = fe.findIndex((x) => x.DBFeld === e);

    if (fe[i].Sichtbar === 1) {
      fe[i].Sichtbar = 0;
    } else {
      fe[i].Sichtbar = 1;
    }

    const fjson = JSON.stringify(fe[i]);
    this.props.websocketSend({ Type: 'bef', Command: 'SaveAnzeigetabelle', Value1: fjson });
  }

  render() {
    let items = [];
    if (this.props.werte.length > 0) {
      items = this.props.werte.map((i) => { // eslint-disable-line arrow-body-style
        return (
          <Checkbox
            key={i.DBFeld}
            label={i.DBFeld}
            checked={i.Sichtbar}
            handleCheckboxChange={this.handleclick.bind(this, i.DBFeld)}
          />
        );
      });
    }

    let cn = 'btn btn-outline-secondary btn-icon ml-1 p-0 pl-1 pr-1';
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
          <GoGear size="1.2em" />
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

DlgFeldwahl.propTypes = {
  werte: PropTypes.arrayOf(PropTypes.object),
  toolTip: PropTypes.string,
  modalID: PropTypes.string,
  label1: PropTypes.string,
  websocketSend: PropTypes.func.isRequired,
  class: PropTypes.string,
};

DlgFeldwahl.defaultProps = {
  werte: [],
  toolTip: 'Feld wahl',
  modalID: 'DlgFeldwahl99',
  label1: '',
  class: '',
};

export default DlgFeldwahl;
