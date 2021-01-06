import React, { Component } from 'react';
import PropTypes from 'prop-types';

class Checkbox extends Component {
  constructor(props) {
    super(props);
    this.state = {
    };

    this.toggleCheckboxChange = this.toggleCheckboxChange.bind(this);
  }

  toggleCheckboxChange() {
    this.props.handleCheckboxChange(this.props.label);
  }

  render() {
    return (
      <div className="checkbox">
        <label className="text-dark pl-2" htmlFor="chkbox">
          <input
            className="mr-2"
            type="checkbox"
            id="chkbox"
            value={this.props.label}
            checked={this.props.checked}
            onChange={this.toggleCheckboxChange}
          />

          {this.props.label}
        </label>
      </div>
    );
  }
}

Checkbox.propTypes = {
  checked: PropTypes.bool,
  label: PropTypes.string,
  handleCheckboxChange: PropTypes.func,
};

Checkbox.defaultProps = {
  checked: false,
  label: '',
  handleCheckboxChange: () => {},
};

export default Checkbox;
