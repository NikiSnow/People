using People.Models;
using People.Services;

namespace People;

public partial class MainPage : ContentPage
{
    private readonly PersonRepository _personRepo;

    public MainPage(PersonRepository personRepo)
    {
        InitializeComponent();
        _personRepo = personRepo;
    }

    private async void OnAddPersonClicked(object sender, EventArgs e)
    {
        string name = nameEntry.Text?.Trim();
        if (!string.IsNullOrEmpty(name))
        {
            await _personRepo.AddNewPerson(name);
            statusLabel.Text = _personRepo.StatusMessage;
            nameEntry.Text = "";
        }
        else
        {
            statusLabel.Text = "Введите имя!";
        }
    }

    private async void OnGetAllPeopleClicked(object sender, EventArgs e)
    {
        List<Person> people = await _personRepo.GetAllPeople();
        peopleListView.ItemsSource = people;
        statusLabel.Text = $"Найдено {people.Count} записей";
    }

    private async void OnDeletePersonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        Person person = (Person)button.BindingContext;

        bool confirm = await DisplayAlert("Подтверждение",
            $"Удалить {person.Name}?",
            "Да", "Нет");

        if (confirm)
        {
            await _personRepo.DeletePerson(person);
            statusLabel.Text = _personRepo.StatusMessage;

            List<Person> people = await _personRepo.GetAllPeople();
            peopleListView.ItemsSource = people;
        }
    }
}
