import { Link } from "react-router-dom";
import { FaUser } from "react-icons/fa";

function Home() {
  return (
    <>
      <section className="heading">
        <h1>Hello , welcome</h1>
        <h1>
          Both Teacher and Student should use the register link to register
        </h1>
      </section>
      <Link to="/register" className="btn btn-reverse btn-block">
        {" "}
        <FaUser /> Register
      </Link>
    </>
  );
}

export default Home;
