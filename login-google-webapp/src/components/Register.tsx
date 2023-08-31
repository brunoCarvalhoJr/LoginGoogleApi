import React, { SyntheticEvent, useState } from 'react';
import PropTypes from 'prop-types';
import axios from 'axios';
import { Navigate } from 'react-router-dom';

const Register = () => {
	
		const [ firstName, setFirstName ] = useState('');
		const [ lastName, setLastName ] = useState('');
		const [ email, setEmail ] = useState('');
		const [ password, setPassword ] = useState('');
		const [ confirmPassword, setConfirmPassword ] = useState('');
		const [ redirect, setRedirect ] = useState(false);

		const submit = async (e: any) => {
			e.preventDefault();

			await axios.post('register', {
				first_name: firstName,
				last_name: lastName,
				email,
				password,
				password_confirm: confirmPassword
			});

			setRedirect(true);
		};

		if(redirect){
			return <Navigate to={'/login'}/>
		}

    return (
      <main className="form-signin w-100 m-auto">
        <form onSubmit={(e) => submit(e)}>
          	<h1 
				className="h3 mb-3 fw-normal"
			>
				Por favor registre-se
			</h1>

			<div className="form-floating">
				<input 
					className="form-control" 
					id="floatingInput" 
					placeholder="Primeiro Nome" 
					onChange={(e) => setFirstName(e.target.value)}
				/>
				<label htmlFor="floatingInput">
					Primeiro Nome
				</label>
			</div>

			<div className="form-floating">
				<input 
					className="form-control" 
					id="floatingInput2" 
					placeholder="Último Nome" 
					onChange={(e) => setLastName(e.target.value)}
				/>
				<label htmlFor="floatingInput2">
					Último Nome
				</label>
			</div>
  
			<div className="form-floating">
				<input 
					type="email" 
					className="form-control" 
					id="floatingInput3" 
					placeholder="nome@examplo.com" 
					onChange={(e) => setEmail(e.target.value)}
				/>
				<label htmlFor="floatingInput3">
					Email
				</label>
			</div>

			<div className="form-floating">
				<input 
					type="password" 
					className="form-control" 
					id="floatingPassword1" 
					placeholder="Senha" 
					autoComplete="confirm-senha1"
					onChange={(e) => setPassword(e.target.value)}
				/>
				<label htmlFor="floatingPassword1">
					Senha
				</label>
			</div>
					
          <div className="form-floating">
            <input 
				type="password" 
				className="form-control" 
				id="floatingPassword2" 
				placeholder="Confirme sua senha" 
				autoComplete="confirm-senha"
				onChange={(e) => setConfirmPassword(e.target.value)}
			/>
            <label htmlFor="floatingPassword2">
				Confirmar Senha
			</label>
          </div>
          
			<button 
				className="btn btn-primary w-100 py-2" 
				type={'submit'}
			>
				Registrar
			</button>
        </form>
      </main>
    )
}

export default Register;