import React, { useState } from 'react';
import axios from 'axios';
import { Navigate } from 'react-router-dom';
import { Link } from 'react-router-dom';
import { GoogleLogin } from '@react-oauth/google';

const Login = () => {

	const [ email, setEmail ] = useState('');
	const [ password, setPassword ] = useState('');
	const [ redirect, setRedirect ] = useState(false);

	const submit = async (e: any) => {
		e.preventDefault();

		const {data} = await axios.post('login', {
			email,
			password
		},{withCredentials: true});

		if(data)
			axios.defaults.headers.common['Authorization'] = `Bearer ${data?.token}`;

		setRedirect(true);
	};

	if(redirect){
		return <Navigate to={'/'}/>
	}

	const onSuccess = async (googleUser: any) => {

		const {data, status} = await axios.post('google-auth', {
			Token: googleUser.credential
		}, {withCredentials: true});

		if(status === 200){
			axios.defaults.headers.common['Authorization'] = `Bearer ${data?.token}`;
			setRedirect(true);
		}
	}

    return (
		<>
			<main className="form-signin w-100 m-auto">
				<form onSubmit={(e) => submit(e)}>
					<h1 
						className="h3 mb-3 fw-normal"
					>
						Login
					</h1>
		
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

					<div className='mb-3'>
						<Link to="/forgot" >Esqueci minha senha</Link>
					</div>
				
					<button 
						className="btn btn-primary w-100 py-2" 
						type={'submit'}
					>
						Entrar
					</button>
				</form>
				<div className='mt-3'>
					<GoogleLogin 
						onSuccess={onSuccess}  
						onError={() => {
							console.log('Login Failed');
						}}
						auto_select={false}
						theme='filled_black'
						width={'300'}
					/>
				</div>
			</main>
		</>
    )
}

export default Login;