import { useState } from "react";
import { FaSign } from "react-icons/fa";
import { useSelector, useDispatch } from "react-redux";
import { login } from "../features/auth/authSlice";

// interface RegisterFormState {
//   FirstName: string,
//   Surname: string,
//   NationIdNumber: string,
//   Password: string,
//   ConfirmPassword: string,
// }

function Login() {
  const [formData, setFormData] = useState({
    NationIdNumber: "",
    Password: "",
  });

  const { NationIdNumber, Password } = formData;

  const dispatch = useDispatch();
  const { user, isLoading, isSuccess, message } = useSelector(
    (state) => state.auth
  );

  const onChange = (e) => {
    setFormData((prevState) => ({
      ...prevState,
      [e.target.name]: e.target.value,
    }));
  };

  const onSubmit = (e) => {
    e.preventDefault();
    const userData = {
      username: NationIdNumber,
      Password,
    };
    dispatch(login(userData));
  };

  return (
    <>
      <section className="heading">
        <h1>
          <FaSign />
          Login
        </h1>
        <p>Please Login </p>
      </section>

      <section className="form">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <input
              type="text"
              className="form-control"
              id="NationIdNumber"
              name="NationIdNumber"
              required
              value={NationIdNumber}
              onChange={onChange}
              placeholder="Enter your NationIdNumber"
            />
          </div>

          <div className="form-group">
            <input
              type="password"
              className="form-control"
              id="Password"
              name="Password"
              required
              value={Password}
              onChange={onChange}
              placeholder="Enter your Password"
            />
          </div>

          <div className="form-group">
            <button className="btn btn-block"> Submit</button>
          </div>
        </form>
      </section>
    </>
  );
}

export default Login;
