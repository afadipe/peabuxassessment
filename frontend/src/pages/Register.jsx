import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { FaUser } from "react-icons/fa";
import { toast } from "react-toastify";
import { useSelector, useDispatch } from "react-redux";
import { register, reset } from "../features/auth/authSlice";
import Spinner from "../components/Spinner";

// interface RegisterFormState {
//   FirstName: string,
//   Surname: string,
//   NationIdNumber: string,
//   Password: string,
//   ConfirmPassword: string,
// }

function Register() {
  const [formData, setFormData] = useState({
    FirstName: "",
    Surname: "",
    NationIdNumber: "",
    Password: "",
    ConfirmPassword: "",
  });

  const { FirstName, Surname, NationIdNumber, Password, ConfirmPassword } =
    formData;

  const dispatch = useDispatch();
  const navigate = useNavigate();
  const { user, isLoading, isError, isSuccess, message } = useSelector(
    (state) => state.auth
  );

  useEffect(() => {
    if (isError) {
      toast.error(message);
    }

    if (isSuccess) {
      navigate("/");
    }
    dispatch(reset());
  }, [isError, isSuccess, user, message, navigate, dispatch]);

  const onChange = (e) => {
    setFormData((prevState) => ({
      ...prevState,
      [e.target.name]: e.target.value,
    }));
  };

  const onSubmit = (e) => {
    e.preventDefault();
    if (Password !== ConfirmPassword) {
      toast.error("Password and ConfirmPassword do not match");
    } else {
      const userData = {
        FirstName,
        Surname,
        NationIdNumber,
        Password,
        ConfirmPassword,
      };
      dispatch(register(userData));
    }
  };

  if (isLoading) {
    return <Spinner />;
  }

  return (
    <>
      <section className="heading">
        <h1>
          <FaUser />
          Register
        </h1>
        <p>Please creat an account</p>
      </section>

      <section className="form">
        <form onSubmit={onSubmit}>
          <div className="form-group">
            <input
              type="text"
              className="form-control"
              id="FirstName"
              name="FirstName"
              required
              value={FirstName}
              onChange={onChange}
              placeholder="Enter your FirstName"
            />
          </div>

          <div className="form-group">
            <input
              type="text"
              className="form-control"
              id="Surname"
              name="Surname"
              required
              value={Surname}
              onChange={onChange}
              placeholder="Enter your Surname"
            />
          </div>

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
            <input
              type="password"
              className="form-control"
              id="ConfirmPassword"
              name="ConfirmPassword"
              required
              value={ConfirmPassword}
              onChange={onChange}
              placeholder="Enter your ConfirmPassword"
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

export default Register;
