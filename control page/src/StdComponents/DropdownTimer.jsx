import React, { Component } from 'react';
import PropTypes from 'prop-types';

class DropdownTimer extends Component {
  handleclick(par) {
    this.props.selection(par);
  }

  render() {
    const items = this.props.values.map((i) => { // eslint-disable-line arrow-body-style
      return (
        <button
          type="button"
          className="dropdown-item p-0 pl-1 pr-1"
          key={i.Nr}
          onClick={this.handleclick.bind(this, i.Nr)}
        >
          {i.Name}
        </button>
      );
    });

    return (
      <div className="">
        <div className="dropdown">
          <button
            className="form-control form-control-sm btn btn-outline-secondary btn-sm dropdown-toggle ml-2 mr-2"
            type="button"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            {this.props.value}
          </button>
          <div className="dropdown-menu scroll-50">
            {items}
          </div>
        </div>
      </div>
    );
  }
}

DropdownTimer.propTypes = {
  values: PropTypes.arrayOf(PropTypes.object),
  value: PropTypes.string,
  selection: PropTypes.func,
};

DropdownTimer.defaultProps = {
  values: [],
  value: '',
  selection: () => {},
};

export default DropdownTimer;
