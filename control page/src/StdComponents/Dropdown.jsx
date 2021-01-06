import React, { Component } from 'react';
import PropTypes from 'prop-types';
import { MdArrowDropDown } from 'react-icons/md';

class Dropdown extends Component {
  handleclick(par) {
    this.props.wahl(par);
  }

  render() {
    let items = null;
    if (this.props.values) {
      items = this.props.values.map((i) => { // eslint-disable-line arrow-body-style
        return (
          <button
            type="button"
            className="dropdown-item p-0 pl-1 pr-1"
            ID={i}
            onClick={this.handleclick.bind(this, i)}
          >
            {i}
          </button>
        );
      });
    }

    let cName = 'btn btn-outline-secondary btn-sm';
    if (this.props.cName !== '') {
      cName = this.props.cName;
    }

    return (
      <div className="">
        <div className="dropdown">
          <button
            className={cName}
            toolTip={this.props.toolTip}
            type="button"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            <div className="d-flex flex-row justify-content-between">
              {this.props.value}
              <MdArrowDropDown size="1.2em" />
            </div>
          </button>
          <div className="dropdown-menu scroll-50">
            {items}
          </div>
        </div>
      </div>
    );
  }
}

Dropdown.propTypes = {
  values: PropTypes.arrayOf(PropTypes.string),
  value: PropTypes.string,
  toolTip: PropTypes.string,
  cName: PropTypes.string,
  wahl: PropTypes.func,
};

Dropdown.defaultProps = {
  toolTip: '',
  values: [],
  value: '',
  cName: '',
  wahl: () => {},
};

export default Dropdown;
