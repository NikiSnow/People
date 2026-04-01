using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SQLite;
using People.Models;
using System.Threading.Tasks;

namespace People.Services;

public class PersonRepository
{
    private SQLiteAsyncConnection _conn;
    private readonly string _dbPath;
    public string StatusMessage { get; private set; } = "";

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    private async Task Init()
    {
        if (_conn != null)
            return;

        _conn = new SQLiteAsyncConnection(_dbPath);
        await _conn.CreateTableAsync<Person>();
    }

    public async Task AddNewPerson(string name)
    {
        try
        {
            await Init();

            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            int result = await _conn.InsertAsync(new Person { Name = name });
            StatusMessage = $"{result} row(s) added (Name: {name})";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to add {name}. Error: {ex.Message}";
        }
    }

    public async Task<List<Person>> GetAllPeople()
    {
        try
        {
            await Init();
            return await _conn.Table<Person>().ToListAsync();
        }
        catch (Exception ex)
        {
            StatusMessage = $"Failed to retrieve data. {ex.Message}";
            return new List<Person>();
        }
    }

    public async Task DeletePerson(Person person)
    {
        try
        {
            await Init();
            int result = await _conn.DeleteAsync(person);
            StatusMessage = result > 0 ? $"Deleted: {person.Name}" : "Failed to delete";
        }
        catch (Exception ex)
        {
            StatusMessage = $"Error deleting: {ex.Message}";
        }
    }
}