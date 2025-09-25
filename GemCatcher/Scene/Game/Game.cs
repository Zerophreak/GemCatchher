using Godot;
using System;

public partial class Game : Node2D
{
	//[Export] private Gem _gem;
	const double GEM_MARGIN = 50.0;
	[Export] private PackedScene _gemScene;
	[Export] private Timer _spawnTimer;
	[Export] private Label _scoreLabel;
	[Export] private AudioStreamPlayer _music;
	[Export] private AudioStreamPlayer2D _effects;
	[Export] private AudioStream _explodeSound;
	// Called when the node enters the scene tree for the first time.

	private int _score = 0;
	public override void _Ready()
	{
		_spawnTimer.Timeout += SpawnGem;
		SpawnGem();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void SpawnGem()
	{
		Rect2 vpr = GetViewportRect();
		Gem gem = (Gem)_gemScene.Instantiate();

		AddChild(gem);

		float rX = (float)GD.RandRange(
			vpr.Position.X + GEM_MARGIN, vpr.End.X - GEM_MARGIN
		);

		gem.Position = new Vector2(rX, -100);
		gem.OnScored += OnScored;
		gem.OnGemOffScreen += GameOver;
	}

	private void OnScored()
	{
		GD.Print("Onscored Received");
		_score += 1;
		_scoreLabel.Text = $"{_score:0000}";
		_effects.Play();

	}

	private void GameOver()
	{
		GD.Print("GameOver");
		foreach (Node node in GetChildren())
		{
			node.SetProcess(false);
		}
		_spawnTimer.Stop();
		_music.Stop();

		_effects.Stop();
		_effects.Stream = _explodeSound;
		_effects.Play();
	}
}
