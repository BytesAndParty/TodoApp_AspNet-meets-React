import React, { useState, useEffect } from 'react';
import { TodoItem } from './models/TodoItem';

const TodoList: React.FC = () => {
	const [items, setItems] = useState<TodoItem[]>([]);
	const [newItemName, setNewItemName] = useState('');

	useEffect(() => {
		fetchItems();
	}, []);

	const fetchItems = async () => {
		try {
			console.log('Fetching todo items...');
			const response = await fetch('http://localhost:5000/api/todoitems');
			if (!response.ok) {
				throw new Error(`Network response was not ok (${response.statusText})`);
			}
			const data: TodoItem[] = await response.json();
			setItems(data);
			console.log('Fetched todo items successfully:', data);
		} catch (error) {
			console.error('There was an error fetching the todo items:', error);
		}
	};

	const addItem = async () => {
		try {
			console.log('Adding new item:', newItemName);
			const response = await fetch('http://localhost:5000/api/todoitems', {
				method: 'POST',
				headers: {
					'Content-Type': 'application/json',
				},
				body: JSON.stringify({ name: newItemName, isComplete: false }),
			});
			if (!response.ok) {
				throw new Error(`Network response was not ok (${response.statusText})`);
			}
			const newItem = await response.json();
			setItems((currentItems) => [...currentItems, newItem]);
			setNewItemName('');
			console.log('Added new item successfully:', newItem);
		} catch (error) {
			console.error('There was an error adding the item:', error);
		}
	};

	const updateItem = async (todo: TodoItem) => {
		try {
			console.log(`Updating item with id ${todo.id}...`);
			const response = await fetch(
				`http://localhost:5000/api/todoitems/${todo.id}`,
				{
					method: 'PUT',
					headers: {
						'Content-Type': 'application/json',
					},
					body: JSON.stringify(todo),
				}
			);
			if (!response.ok) {
				throw new Error(`Network response was not ok (${response.statusText})`);
			}
			setItems((currentItems) =>
				currentItems.map((item) =>
					item.id === todo.id ? { ...item, ...todo } : item
				)
			);
			console.log(`Updated item with id ${todo.id} successfully.`);
		} catch (error) {
			console.error(
				`There was an error updating the item with id ${todo.id}:`,
				error
			);
		}
	};

	const deleteItem = async (id: number) => {
		try {
			console.log(`Deleting item with id ${id}...`);
			const response = await fetch(
				`http://localhost:5000/api/todoitems/${id}`,
				{
					method: 'DELETE',
				}
			);
			if (!response.ok) {
				throw new Error(`Network response was not ok (${response.statusText})`);
			}
			setItems((currentItems) => currentItems.filter((item) => item.id !== id));
			console.log(`Deleted item with id ${id} successfully.`);
		} catch (error) {
			console.error(
				`There was an error deleting the item with id ${id}:`,
				error
			);
		}
	};

	return (
		<div>
			<h2>Meine To-Do-Liste</h2>
			<input
				type='text'
				value={newItemName}
				onChange={(e) => setNewItemName(e.target.value)}
			/>
			<button onClick={addItem}>Hinzufügen</button>
			<ul>
				{items.map((item) => (
					<li key={item.id}>
						{item.name}
						<input
							type='checkbox'
							checked={item.isComplete}
							onChange={(e) =>
								updateItem({ ...item, isComplete: e.target.checked })
							}
						/>

						<button onClick={() => deleteItem(item.id)}>Löschen</button>
					</li>
				))}
			</ul>
		</div>
	);
};

export default TodoList;
