import React, { Component } from 'react';
import PropTypes from 'prop-types';

class DropdownFont extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };
  }

  handleclick(e) {
    this.props.selection(e);
  }

  render() {
    const items = this.props.values.map((i) => { // eslint-disable-line arrow-body-style
      return (
        <button
          type="button"
          className="dropdown-item p-0 pl-1 pr-1"
          style={{ fontFamily: i }}
          key={i}
          onClick={this.handleclick.bind(this, i)}
        >
          {i}
        </button>
      );
    });

    return (
      <div className="">
        <div className="dropdown p-0">
          <button
            className="btn btn-outline-secondary btn-sm dropdown-toggle"
            type="button"
            data-toggle="dropdown"
            aria-haspopup="true"
            aria-expanded="false"
          >
            {this.props.value}
          </button>
          <div className="dropdown-menu">
            {items}
          </div>
        </div>
      </div>
    );
  }
}

DropdownFont.propTypes = {
  value: PropTypes.string,
  values: PropTypes.arrayOf(PropTypes.string),
  selection: PropTypes.func,
};

DropdownFont.defaultProps = {
  value: '',
  values: [],
  selection: () => {},
};

export default DropdownFont;
