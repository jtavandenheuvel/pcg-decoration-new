//-----------------------------------------------
// XUI - Game1.cs
// Copyright (C) Peter Reid. All rights reserved.
//-----------------------------------------------

using Microsoft.Xna.Framework;

// class Game1
public class Game1 : Game
{
	// Game1
	public Game1()
		: base()
	{
		Graphics = new GraphicsDeviceManager( this );

		Graphics.PreferredBackBufferWidth = 1280;
		Graphics.PreferredBackBufferHeight = 720;

		Content.RootDirectory = "Content";
	}

	// Initialize
	protected override void Initialize()
	{
		base.Initialize();
	}

	// LoadContent
	protected override void LoadContent()
	{
		// create the GameInput - UI depends on this
		GameInput = new GameInput( (int)E_UiButton.Count, (int)E_UiAxis.Count );

		// setup the UI's input mappings
		_UI.SetupControls( GameInput );

		// setup the UI with default settings
		_UI.Startup( this, GameInput );

		// add the initial UI screen
		_UI.Screen.AddScreen( new UI.ScreenTest() );

		base.LoadContent();
	}

	// UnloadContent
	protected override void UnloadContent()
	{
		// shutdown the UI
		_UI.Shutdown();

		base.UnloadContent();
	}

	// Update
	protected override void Update( GameTime gameTime )
	{
		float frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

		// update the GameInput
		GameInput.Update( frameTime );

		// update the UI
		_UI.Sprite.BeginUpdate();
		_UI.Screen.Update( frameTime );

		base.Update( gameTime );
	}

	// Draw
	protected override void Draw( GameTime gameTime )
	{
		GraphicsDevice.Clear( Color.CornflowerBlue );

		// render the UI - default 2D render pass
		_UI.Sprite.Render( 0 );

		base.Draw( gameTime );
	}

	//
	private GraphicsDeviceManager		Graphics;
	private GameInput					GameInput;
	//
};
