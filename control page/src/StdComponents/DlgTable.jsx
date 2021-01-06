import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { BsTable } from 'react-icons/bs';
import { propTypes } from 'qrcode.react';

class DlgTable extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleclick(manID) {
    this.props.wahl({ wahl: manID, funcID: this.props.funcID });
  }

  render() {
    const items = this.props.data.map((i) => { // eslint-disable-line arrow-body-style
      return (
        <button
          type="button"
          className="dropdown-item p-0 pl-1 pr-1"
          key={i.ID}
          data-dismiss="modal"
          onClick={this.handleclick.bind(this, i.ID)}
        >
          {i.Name}
        </button>
      );
    });

    let buttenstyle = 'btn btn-outline-secondary p-0 pl-1 pr-1 pb-0';
    if (this.props.className !== '') {
      buttenstyle = this.props.className;
    }

    let icon = '';
    if (this.props.icon !== '') {
      icon = (
        <BsTable />
      );
    }
    if (this.props.reacticon !== '') {
      icon = this.props.reacticon;
    }

    return (
      <div>
        <button
          type="button"
          className={buttenstyle}
          data-placement="right"
          title={this.props.toolTip}
          data-toggle="modal"
          data-target={`#${this.props.modalID}`}
          disabled={this.props.disabled}
        >
          {icon}
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

DlgTable.propTypes = {
  modalID: PropTypes.string,
  label1: PropTypes.string,
  toolTip: PropTypes.string,
  icon: PropTypes.string,
  // eslint-disable-next-line react/forbid-prop-types
  reacticon: propTypes.object,
  funcID: PropTypes.number,
  disabled: PropTypes.bool,
  className: PropTypes.string,
  data: PropTypes.arrayOf(PropTypes.object),
  wahl: PropTypes.func,
};

DlgTable.defaultProps = {
  modalID: 'filedlg',
  label1: 'Tabelle',
  toolTip: '',
  icon: '',
  reacticon: '',
  funcID: 0,
  disabled: false,
  className: '',
  data: [],
  wahl: () => {},
};

export default DlgTable;
